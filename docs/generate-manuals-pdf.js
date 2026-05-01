const puppeteer = require('puppeteer');
const fs = require('fs');
const path = require('path');
const { marked } = require('marked');

const manuals = [
    { md: 'Manual_Administrador.md', html: 'Manual_Administrador.html', pdf: 'Manual_Administrador.pdf', title: 'Manual de Usuario - Administrador' },
    { md: 'Manual_Supervisor.md', html: 'Manual_Supervisor.html', pdf: 'Manual_Supervisor.pdf', title: 'Manual de Usuario - Supervisor' }
];

const cssStyles = `
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #1f2937;
            font-size: 11pt;
        }
        
        h1 {
            font-size: 24pt;
            color: #047857;
            border-bottom: 3px solid #047857;
            padding-bottom: 10px;
            margin-bottom: 5px;
            margin-top: 0;
        }
        
        h2 {
            font-size: 16pt;
            color: #047857;
            border-bottom: 1px solid #d1d5db;
            padding-bottom: 6px;
            margin-top: 30px;
            margin-bottom: 12px;
        }
        
        h3 {
            font-size: 13pt;
            color: #374151;
            margin-top: 20px;
            margin-bottom: 10px;
        }
        
        h4 {
            font-size: 11pt;
            color: #4b5563;
            margin-top: 15px;
            margin-bottom: 8px;
        }
        
        p {
            margin-bottom: 10px;
        }
        
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 15px 0;
            font-size: 10pt;
        }
        
        th {
            background-color: #047857;
            color: white;
            padding: 8px 12px;
            text-align: left;
            font-weight: 600;
        }
        
        td {
            padding: 6px 12px;
            border: 1px solid #d1d5db;
        }
        
        tr:nth-child(even) {
            background-color: #f3f4f6;
        }
        
        tr:nth-child(odd) {
            background-color: #ffffff;
        }
        
        code {
            background-color: #f3f4f6;
            padding: 2px 6px;
            border-radius: 3px;
            font-family: 'Consolas', 'Courier New', monospace;
            font-size: 10pt;
        }
        
        pre {
            background-color: #1f2937;
            color: #e5e7eb;
            padding: 12px 16px;
            border-radius: 6px;
            overflow-x: auto;
            margin: 12px 0;
        }
        
        pre code {
            background: none;
            padding: 0;
            color: inherit;
        }
        
        blockquote {
            border-left: 4px solid #047857;
            background-color: #ecfdf5;
            padding: 10px 15px;
            margin: 15px 0;
            color: #065f46;
        }
        
        ul, ol {
            margin-left: 20px;
            margin-bottom: 10px;
        }
        
        li {
            margin-bottom: 4px;
        }
        
        strong {
            color: #047857;
        }
        
        hr {
            border: none;
            border-top: 1px solid #d1d5db;
            margin: 25px 0;
        }
        
        .cover-page {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            min-height: 80vh;
            text-align: center;
        }
        
        .cover-page h1 {
            font-size: 32pt;
            border: none;
            margin-bottom: 20px;
        }
        
        .cover-page .subtitle {
            font-size: 16pt;
            color: #6b7280;
            margin-bottom: 40px;
        }
        
        .cover-page .info {
            font-size: 11pt;
            color: #9ca3af;
        }
        
        .cover-page .logo {
            font-size: 48pt;
            margin-bottom: 30px;
        }
        
        a {
            color: #047857;
            text-decoration: none;
        }
        
        .page-break {
            page-break-before: always;
        }
    </style>
`;

async function generatePDF(manual) {
    console.log(`Converting ${manual.md} to HTML...`);
    
    const mdContent = fs.readFileSync(path.join(__dirname, manual.md), 'utf8');
    const htmlContent = marked.parse(mdContent);
    
    const fullHtml = `
        <!DOCTYPE html>
        <html lang="es">
        <head>
            <meta charset="UTF-8">
            <title>${manual.title}</title>
            ${cssStyles}
        </head>
        <body>
            ${htmlContent}
        </body>
        </html>
    `;
    
    fs.writeFileSync(path.join(__dirname, manual.html), fullHtml);
    console.log(`HTML saved: ${manual.html}`);
    
    console.log(`Generating PDF: ${manual.pdf}...`);
    
    const browser = await puppeteer.launch({
        headless: 'new',
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });
    
    const page = await browser.newPage();
    await page.setContent(fullHtml, { waitUntil: 'networkidle0' });
    
    await page.pdf({
        path: path.join(__dirname, manual.pdf),
        format: 'A4',
        margin: {
            top: '25mm',
            right: '20mm',
            bottom: '25mm',
            left: '20mm'
        },
        printBackground: true
    });
    
    await browser.close();
    console.log(`PDF generated successfully: ${manual.pdf}`);
}

(async () => {
    for (const manual of manuals) {
        await generatePDF(manual);
        console.log('---');
    }
    console.log('All PDFs generated successfully!');
})();
