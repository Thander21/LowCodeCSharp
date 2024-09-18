import * as vscode from 'vscode';

export function activate(context) {
    let disposable = vscode.commands.registerCommand('extension.showDiagram', () => {
        const panel = vscode.window.createWebviewPanel(
            'diagramView',
            'Diagrama de Fluxo',
            vscode.ViewColumn.One,
            {}
        );

        panel.webview.html = getWebviewContent();
    });

    context.subscriptions.push(disposable);
}

function getWebviewContent() {
    return `<!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Diagrama de Fluxo</title>
    </head>
    <body>
        <div id="diagram"></div>
        <script>
            // Aqui você pode inserir o código para renderizar o diagrama
            // usando os dados fornecidos pelo backend C#
        </script>
    </body>
    </html>`;
}
