/**
 * main.js
 * Felis Sapiens - UI logic and Chart initialization
 */

document.addEventListener('DOMContentLoaded', () => {

    // --- Hamburger Menu ---
    const hamburger = document.querySelector('.hamburger-menu');
    const navLinks = document.querySelector('.nav-links');
    if (hamburger && navLinks) {
        hamburger.addEventListener('click', () => {
            hamburger.classList.toggle('is-active');
            navLinks.classList.toggle('is-active');
        });
        
        // Close on link click
        navLinks.querySelectorAll('a').forEach(a => {
            a.addEventListener('click', () => {
                hamburger.classList.remove('is-active');
                navLinks.classList.remove('is-active');
            });
        });
        
        // Close on outside click
        document.addEventListener('click', (event) => {
            if (navLinks.classList.contains('is-active')) {
                const isClickInsideMenu = navLinks.contains(event.target);
                const isClickOnHamburger = hamburger.contains(event.target);
                if (!isClickInsideMenu && !isClickOnHamburger) {
                    hamburger.classList.remove('is-active');
                    navLinks.classList.remove('is-active');
                }
            }
        });
    }

    /* --- Intersection Observer for Scroll Animations --- */
    const observerOptions = {
        root: null,
        rootMargin: '0px 0px -10% 0px',
        threshold: 0
    };

    const animateOnScrollObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('is-visible');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    const animateElements = document.querySelectorAll('.animate-on-scroll');
    animateElements.forEach(el => animateOnScrollObserver.observe(el));


    /* --- Chart.js Setup using simulation data.js --- */
    // Provided by ../vizualizace/data.js (global object simulationData)
    if (typeof simulationData !== 'undefined') {
        initCharts(simulationData);
    } else if (typeof exportedData !== 'undefined') {
        initCharts(exportedData);
    } else {
        console.error("Simulation data missing! Ensure '../vizualizace/data.js' is loaded before main.js.");
    }

    /* --- Custom Video Player Logic (Multi-Instance) --- */
    const observatories = document.querySelectorAll('.video-observatory');
    
    observatories.forEach(obs => {
        const video = obs.querySelector('.video-player');
        const trigger = obs.querySelector('.play-trigger');
        const overlayBox = obs.querySelector('.video-overlay-box');
        
        if (video && trigger) {
            const iconPlay = trigger.querySelector('.icon-play');
            const iconPause = trigger.querySelector('.icon-pause');
            
            trigger.addEventListener('click', () => {
                if (video.paused) {
                    video.play();
                    video.classList.add('is-playing');
                    trigger.classList.add('is-playing');
                    if (overlayBox) overlayBox.classList.add('fade-out');
                    
                    iconPlay.style.display = 'none';
                    iconPause.style.display = 'block';
                } else {
                    video.pause();
                    video.classList.remove('is-playing');
                    trigger.classList.remove('is-playing');
                    if (overlayBox) overlayBox.classList.remove('fade-out');
                    
                    iconPause.style.display = 'none';
                    iconPlay.style.display = 'block';
                }
            });
        }
    });

    /* --- Form Progression Logic --- */
    const step1 = document.getElementById('step-1');
    const step2 = document.getElementById('step-2');
    const step3 = document.getElementById('step-3');
    const step4 = document.getElementById('step-4');
    
    const btnSubmit1 = document.getElementById('btn-submit-phase1');
    const btnSubmit2 = document.getElementById('btn-submit-phase2');
    const btnSubmit3 = document.getElementById('btn-submit-phase3');
    const btnSubmit4 = document.getElementById('btn-submit-phase4');
    const btnBack1   = document.getElementById('btn-back-phase1');
    const btnBack2   = document.getElementById('btn-back-phase2');
    const btnReset   = document.getElementById('btn-reset-form');
    const additionalMessage = document.getElementById('additionalMessage');

    const msgPhase2 = document.getElementById('phase-2-msg');
    const msgPhase3 = document.getElementById('phase-3-msg');
    const msgPhase4 = document.getElementById('phase-4-msg');
    const formTop   = document.getElementById('dossier-form');
    const applicantNameInput = document.getElementById('applicantName');

    function setPhase(phase) {
        localStorage.setItem('felisSapiensPhase', phase);
    }

    function getPhase() {
        return localStorage.getItem('felisSapiensPhase') || '1';
    }

    function saveApplicantName() {
        if (applicantNameInput && applicantNameInput.value.trim()) {
            localStorage.setItem('felisSapiensName', applicantNameInput.value.trim());
        }
    }
    
    function injectApplicantName() {
        const name = localStorage.getItem('felisSapiensName') || 'Applicant';
        document.querySelectorAll('.applicant-name-display').forEach(el => {
            el.textContent = name;
        });
    }

    function renderPhase(phase, skipScroll = false) {
        if (!step1) return; // fail safe
        
        step1.style.display = 'none';
        step2.style.display = 'none';
        if (step3) step3.style.display = 'none';
        if (step4) step4.style.display = 'none';

        injectApplicantName();

        const dossierIntro = document.getElementById('dossier-intro-text');
        const applicantName = localStorage.getItem('felisSapiensName') || '';

        if (phase === '1') {
            step1.style.display = 'block';
            if (applicantNameInput && applicantName) applicantNameInput.value = applicantName;
            if (dossierIntro) {
                const greeting = applicantName ? `<span style="color: var(--color-accent); font-family: var(--font-serif); font-size: 1.1em;">Welcome back, ${applicantName}.</span><br>` : '';
                dossierIntro.innerHTML = `${greeting}If our <a href="#ethics-concept" style="text-decoration: underline; color: var(--color-accent); font-weight: bold;">ethical approach</a> resonates with you, we invite you to submit your preliminary details below. We value candidates who share our vision.`;
                dossierIntro.style.color = 'rgba(255,255,255,0.8)';
            }
        } else if (phase === '2') {
            step2.style.display = 'block';
            if (dossierIntro) {
                const greeting = applicantName ? `Dear ${applicantName}, thank you` : 'Thank you';
                dossierIntro.innerHTML = `${greeting} immensely for initiating this process. Your preliminary identity is securely recorded. We sincerely appreciate your interest in the Felis Sapiens lineage and deeply value candidates who share our vision. To continue gently, please share the environmental context of your household below.`;
                dossierIntro.style.color = 'rgba(255,255,255,0.9)';
            }
            if (msgPhase2) msgPhase2.innerHTML = ''; // Keep clean of legacy alerts
        } else if (phase === '3') {
            if (step3) step3.style.display = 'block';
            if (dossierIntro) {
                const greeting = applicantName ? `Thank you, ${applicantName}, for detailing` : 'Thank you for detailing';
                dossierIntro.innerHTML = `${greeting} your environment. We are truly reading every word and appreciate your transparency. As a final step, we kindly ask you to share your broader philosophy and expectations. This helps us ensure the perfect, seamless synergy between you and the feline intellect.`;
                dossierIntro.style.color = 'rgba(255,255,255,0.9)';
            }
            if (msgPhase3) msgPhase3.innerHTML = '';
        } else if (phase === '4') {
            if (step4) step4.style.display = 'block';
            if (dossierIntro) {
                dossierIntro.innerHTML = `<span style="color: var(--color-accent); font-size: var(--text-md); display: block; margin-bottom: var(--space-xs); font-family: var(--font-serif);">Dossier Officially Secured</span>Thank you beyond words. Your behavioral responses and contextual framework have been successfully preserved.<br><br>Your formal registration is now complete. We treat all subjective data with uncompromising ethical standards. All subsequent communication is handled strictly manually by our human team.<br><br>If any new thoughts or questions arise, you may append them using the space below, or reach out directly at <a href="mailto:protocol@felis-sapiens.com" style="color: var(--color-accent); font-weight: bold; text-decoration: underline;">protocol@felis-sapiens.com</a>`;
                dossierIntro.style.color = 'var(--color-bg)';
            }
            if (msgPhase4) msgPhase4.innerHTML = '';
        }

        if (!skipScroll && formTop) {
            formTop.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }

    function validatePhase(stepEl) {
        const reqs = stepEl.querySelectorAll('.form-control[required]');
        for (const el of reqs) {
            if (!el.value.trim()) {
                el.reportValidity();
                return false;
            }
        }
        return true;
    }

    // Initialize on load
    if (step1) {
        renderPhase(getPhase(), true);
        
        if (btnSubmit1) btnSubmit1.addEventListener('click', () => {
            if (!validatePhase(step1)) return;
            saveApplicantName();
            setPhase('2');
            renderPhase('2');
        });
        
        if (btnBack1) btnBack1.addEventListener('click', () => {
            // Retain name if they go back
            setPhase('1');
            renderPhase('1');
        });

        if (btnSubmit2) btnSubmit2.addEventListener('click', () => {
            if (!validatePhase(step2)) return;
            setPhase('3');
            renderPhase('3');
        });
        
        if (btnBack2) btnBack2.addEventListener('click', () => {
            setPhase('2');
            renderPhase('2');
        });
        
        if (btnSubmit3) btnSubmit3.addEventListener('click', () => {
            if (!validatePhase(step3)) return;
            setPhase('4');
            renderPhase('4');
        });

        if (btnSubmit4) btnSubmit4.addEventListener('click', () => {
            if (additionalMessage && additionalMessage.value.trim() !== '') {
                const originalText = btnSubmit4.innerText;
                btnSubmit4.innerText = 'Appended to your file ✓';
                additionalMessage.value = '';
                setTimeout(() => {
                    if(btnSubmit4.innerText === 'Appended to your file ✓') btnSubmit4.innerText = originalText;
                }, 3000);
            }
        });

        if (btnReset) btnReset.addEventListener('click', () => {
            localStorage.removeItem('felisSapiensPhase');
            localStorage.removeItem('felisSapiensName');
            if(applicantNameInput) applicantNameInput.value = '';
            renderPhase('1');
        });
    }
});

function initCharts(data) {
    // Shared Styling from CSS variables (Fallback if CSS variables cannot be parsed directly by Chart.js)
    const colorBg = '#F6F8FB';
    const colorMidnight = '#1A1A2E';
    const colorMocha = '#A47764';
    const colorSage = '#2D5A4A';
    
    // Global Chart.js styling overrides for Skandi Luxury
    Chart.defaults.color = colorMidnight;
    Chart.defaults.font.family = "'Inter', sans-serif";
    Chart.defaults.font.size = 13;
    Chart.defaults.plugins.tooltip.backgroundColor = colorMidnight;
    Chart.defaults.plugins.tooltip.titleFont = { family: "'Playfair Display', serif", size: 16 };
    Chart.defaults.plugins.legend.labels.usePointStyle = true;
    Chart.defaults.plugins.legend.labels.boxWidth = 8;
    
    const strategies = Object.keys(data);
    const generations = data[strategies[0]].map((_, i) => `Gen ${i + 1}`);

    // Map each strategy to a specific, elegant color
    const palette = [colorSage, colorMocha, colorMidnight];
    
    const datasetsMaxIQ = [];
    const datasetsAvgIQ = [];
    const datasetsDiversity = [];

    strategies.forEach((strat, idx) => {
        const c = palette[idx % palette.length];
        const stratData = data[strat];
        
        // Remove trailing "BreedingStrategy" for cleaner labels
        const cleanLabel = strat.replace('BreedingStrategy', '').replace(/([A-Z])/g, ' $1').trim();

        datasetsMaxIQ.push({
            label: cleanLabel,
            data: stratData.map(d => d.MedianMaxIq || d.MaxIq),
            borderColor: c,
            backgroundColor: c,
            borderWidth: 2,
            tension: 0.3,
            yAxisID: 'y'
        });

        datasetsAvgIQ.push({
            label: cleanLabel,
            data: stratData.map(d => d.MedianAvgIq || d.AverageIq),
            borderColor: c,
            backgroundColor: c,
            borderWidth: 2,
            tension: 0.3
        });

        datasetsDiversity.push({
            label: cleanLabel,
            data: stratData.map(d => d.MedianGeneticDiversity || d.GeneticDiversity),
            borderColor: c,
            backgroundColor: c,
            borderWidth: 2,
            tension: 0.3
        });
    });

    const getCommonOptions = () => ({
        responsive: true,
        maintainAspectRatio: false,
        layout: {
            padding: { bottom: 15 }
        },
        plugins: {
            legend: { position: 'bottom' }
        },
        scales: {
            x: { grid: { display: false } },
            y: { 
                type: 'linear',
                display: true,
                position: 'left', 
                grid: { color: 'rgba(26,26,46,0.05)' },
                title: { display: true, text: 'IQ', font: { style: 'italic' } }
            },
            yRatio: {
                type: 'linear',
                display: true,
                position: 'right',
                grid: { display: false },
                title: { display: true, text: '1:n (Rarity)', font: { style: 'italic' } },
                afterDataLimits: (scale) => {
                    const yScale = scale.chart.scales.y;
                    if (yScale) {
                        scale.min = yScale.min;
                        scale.max = yScale.max;
                    }
                },
                afterBuildTicks: (scale) => {
                    const yScale = scale.chart.scales.y;
                    if (yScale && yScale.ticks) {
                        scale.ticks = yScale.ticks.map(t => ({ value: t.value }));
                    }
                },
                ticks: {
                    callback: function(value) {
                        if (value <= 100) return "1:1";
                        const z = (value - 100) / 15;
                        const t = 1.0 / (1.0 + 0.2316419 * z);
                        const d = 0.3989423 * Math.exp(-z * z / 2.0);
                        const prob = d * t * (0.3193815 + t * (-0.3565638 + t * (1.781478 + t * (-1.821256 + t * 1.330274))));
                        const n = Math.round(1 / prob);
                        if (n >= 1000000) return "1:" + (n/1000000).toFixed(1) + "M";
                        if (n >= 1000) return "1:" + Math.round(n/1000) + "k";
                        return "1:" + n;
                    }
                }
            }
        }
    });

    // 1. Max Peak IQ
    new Chart(document.getElementById('maxIqChart').getContext('2d'), {
        type: 'line',
        data: { labels: generations, datasets: datasetsMaxIQ },
        options: getCommonOptions()
    });

    // 2. Average Population IQ
    new Chart(document.getElementById('avgIqChart').getContext('2d'), {
        type: 'line',
        data: { labels: generations, datasets: datasetsAvgIQ },
        options: getCommonOptions()
    });

    // 3. Genetic Diversity
    new Chart(document.getElementById('diversityChart').getContext('2d'), {
        type: 'line',
        data: { labels: generations, datasets: datasetsDiversity },
        options: {
             responsive: true,
             maintainAspectRatio: false,
             layout: { padding: { bottom: 15 } },
             plugins: {
                 legend: { position: 'bottom' }
             },
             scales: {
                  x: { grid: { display: false } },
                  y: { 
                      position: 'left',
                      grid: { color: 'rgba(26,26,46,0.05)' },
                      min: 0,
                      max: 1 // Diversity is usually 0.0 - 1.0 logic
                  }
             }
        }
    });
}
