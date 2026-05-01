const puppeteer = require('puppeteer');

(async () => {
    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });
    const page = await browser.newPage();
    
    await page.goto(`file://${process.cwd()}/Manual_Usuario_BananaGestion_Frontend.html`, {
        waitUntil: 'networkidle0'
    });
    
    await page.pdf({
        path: 'Manual_Usuario_BananaGestion_Frontend.pdf',
        format: 'A4',
        margin: {
            top: '20mm',
            right: '15mm',
            bottom: '20mm',
            left: '15mm'
        },
        printBackground: true
    });
    
    await browser.close();
    console.log('PDF generated successfully: Manual_Usuario_BananaGestion_Frontend.pdf');
})();
