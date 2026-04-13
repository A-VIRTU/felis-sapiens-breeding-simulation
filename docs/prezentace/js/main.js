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
    }

    /* --- Intersection Observer for Scroll Animations --- */
    const observerOptions = {
        root: null,
        rootMargin: '0px',
        threshold: 0.15
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
