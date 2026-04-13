using CatBreeding.Core.Domain;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add CORS strictly for our Vercel frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFelisSapiensUI", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "https://felissapiens.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure EventStoreDB
var esdbRawConn = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
var settings = EventStoreClientSettings.Create(esdbRawConn);
var client = new EventStoreClient(settings);
builder.Services.AddSingleton(client);

var app = builder.Build();
app.UseCors("AllowFelisSapiensUI");

app.MapPost("/api/dossier/submit", async (JsonDocument payload, EventStoreClient esClient) =>
{
    try
    {
        var action = payload.RootElement.GetProperty("action").GetString();
        if (action == "APPEND_NOTE")
        {
            var appendEvent = new ApplicantDossierAppended
            {
                ApplicantEmail = payload.RootElement.GetProperty("applicantEmail").GetString() ?? "unknown",
                Message = payload.RootElement.GetProperty("message").GetString() ?? ""
            };

            var eventData = new EventData(
                Uuid.NewUuid(),
                nameof(ApplicantDossierAppended),
                JsonSerializer.SerializeToUtf8Bytes(appendEvent)
            );

            // Stream id based on email hash or just global dossiers
            var streamId = $"Applicant-{appendEvent.ApplicantEmail.Replace("@", "-").Replace(".", "-")}";
            await esClient.AppendToStreamAsync(streamId, StreamState.Any, new[] { eventData });
            
            return Results.Ok(new { Status = "Appended" });
        }
    }
    catch { /* Not an append action, proceed to initial submission */ }

    // Parse the full dossier
    var root = payload.RootElement;
    var submittedEvent = new ApplicantDossierSubmitted
    {
        ApplicantName = root.TryGetProperty("applicantName", out var nm) ? nm.GetString() ?? "" : "",
        ApplicantEmail = root.TryGetProperty("applicantEmail", out var em) ? em.GetString() ?? "" : "",
        AcquisitionIntent = root.TryGetProperty("acquisitionIntent", out var ai) ? ai.GetString() ?? "" : "",
        EnvTopography = root.TryGetProperty("envTopography", out var et) ? et.GetString() ?? "" : "",
        EnvDemographics = root.TryGetProperty("envDemographics", out var ed) ? ed.GetString() ?? "" : "",
        Waitlist2026 = root.TryGetProperty("waitlist2026", out var wl) && (wl.ValueKind == JsonValueKind.True || wl.ValueKind == JsonValueKind.String && wl.GetString() == "on"),
        CalibrationConsent = root.TryGetProperty("calibrationConsent", out var cc) && (cc.ValueKind == JsonValueKind.True || cc.ValueKind == JsonValueKind.String && cc.GetString() == "on"),
        PublicCredit = root.TryGetProperty("publicCredit", out var pc) ? pc.GetString() : null,
        HumanElementDrive = root.TryGetProperty("humanElementDrive", out var hd) ? hd.GetString() ?? "" : "",
        HumanElementEmpathy = root.TryGetProperty("humanElementEmpathy", out var he) ? he.GetString() ?? "" : "",
        GeneralNotes = root.TryGetProperty("generalNotes", out var gn) ? gn.GetString() : null,
    };

    var submitEventData = new EventData(
        Uuid.NewUuid(),
        nameof(ApplicantDossierSubmitted),
        JsonSerializer.SerializeToUtf8Bytes(submittedEvent)
    );

    var mainStreamId = $"Applicant-{submittedEvent.ApplicantEmail.Replace("@", "-").Replace(".", "-")}";
    await esClient.AppendToStreamAsync(mainStreamId, StreamState.Any, new[] { submitEventData });

    return Results.Ok(new { Status = "Secured" });
});

app.Run();
