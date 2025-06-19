// Sayfa yüklendiğinde animasyonları başlat
window.addEventListener('load', function () {
    // İstatistik animasyonları
    const statNumbers = document.querySelectorAll('.stat-number');
    statNumbers.forEach(stat => {
        const finalValue = stat.textContent;
        stat.style.opacity = '1';
    });
});