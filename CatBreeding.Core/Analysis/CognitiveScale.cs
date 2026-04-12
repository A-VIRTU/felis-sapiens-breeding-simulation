// =================================================================
// CatBreeding.Core/Analysis/FitnessConverter.cs
// =================================================================
namespace CatBreeding.Core.Analysis
{
    /// <summary>
    /// Provides a static, ordered collection of cognitive benchmarks across different species
    /// and human developmental stages, mapped to an equivalent human IQ scale.
    /// NOTE: These values are estimations based on various scientific studies and popular science sources.
    /// Direct IQ measurement across species is not possible; these are analogies for contextual understanding.
    /// </summary>
    public static class CognitiveScale
    {
        /// <summary>
        /// Returns a sorted dictionary of cognitive benchmarks.
        /// The key is the estimated equivalent IQ score, and the value is a descriptive label.
        /// </summary>
        /// <returns>A SortedDictionary mapping estimated IQ to a description.</returns>
        public static SortedDictionary<int, string> GetCognitiveBenchmarks()
        {
            return new SortedDictionary<int, string>
            {
                // --- Animal Kingdom Benchmarks ---
                { 1, "Hmyz (např. včela, mravenec) - základní instinkty, navigace" },
                { 5, "Průměrná ryba (např. zlatá rybka) - paměť v řádu měsíců" },
                { 8, "Průměrný plaz (např. ještěrka)" },
                { 12, "Průměrný hlodavec (např. myš, potkan) - schopnost učit se v bludišti" },
                { 15, "Průměrný pes (méně inteligentní plemena)" },
                { 18, "Prase - považováno za jedno z nejchytřejších domácích zvířat" },
                { 20, "Průměrný pes (inteligentní plemena jako border kolie)" },
                { 25, "Průměrná dospělá kočka (kognitivní úroveň cca 2letého dítěte)" },
                { 35, "Vrána, havran - používání nástrojů, plánování" },
                { 40, "Slon - vynikající paměť, komplexní sociální struktury" },
                { 50, "Papoušek šedý (Žako) - schopnost abstrakce, učení lidské řeči" },
                { 60, "Průměrný delfín skákavý - sebeuvědomění, komplexní komunikace" },
                { 75, "Gorila (např. Koko, která se naučila znakový jazyk)" },
                { 85, "Průměrný šimpanz nebo orangutan - pokročilé používání nástrojů" },

                // --- Human Developmental Benchmarks ---
                { 10, "Lidské novorozeně (0-6 měsíců) - reflexy, rozpoznávání tváří" },
                { 15, "Lidské batole (1 rok) - první slova, chůze" },
                { 25, "Lidské dítě (2 roky) - jednoduché věty, řešení problémů" },
                { 40, "Lidské dítě (3 roky) - pokládání otázek, základní počty" },
                { 55, "Lidské dítě (4 roky) - rozvinutá řeč, komplexní hry" },
                { 70, "Lidské dítě (6 let) - začátek školní docházky, čtení, psaní" },
                { 85, "Lidské dítě (10 let) - abstraktní myšlení, logické operace" },
                { 95, "Lidský teenager (14-16 let) - téměř plně rozvinuté kognitivní schopnosti" },

                // --- Standard Human IQ Scale ---
                { 100, "Průměrný dospělý člověk" },
                { 115, "Nadprůměrný člověk (např. vysokoškolák) - 1 směrodatná odchylka" },
                { 130, "Vysoce inteligentní člověk (člen Mensy) - 2 směrodatné odchylky" },
                { 145, "Génius (např. Albert Einstein, odhad) - 3 směrodatné odchylky" }
            };
        }
    }
}