import * as vscode from 'vscode';
import * as path from 'path';
import * as childProcess from 'child_process';

export function activate(context: vscode.ExtensionContext) {
  let disposable = vscode.commands.registerCommand('extension.analyzeProject', () => {
    // Obter o workspace atual
    const workspace = vscode.workspace;

    // Obter o caminho da pasta do projeto
    let projectPath: string | undefined = workspace.workspaceFolders?.[0].uri.fsPath;

    // Verificar se projectPath é definido
    if (projectPath) {
      // Obter o caminho do arquivo C#
      const filePath = path.join(projectPath, 'LowCodeCSharp.cs');

      // Obter o caminho do projeto C#
      const csprojPath = path.join(projectPath, 'LowCodeCSharpConsole', 'LowCodeCSharpConsole.csproj');

      // Executar o processo C#
      const process = childProcess.spawn('dotnet', ['run', '--project', csprojPath], {
        cwd: path.join(projectPath, 'LowCodeCSharpConsole') // Diretório do projeto C#
      });

      // Capturar a saída do processo C#
      let output = '';
      process.stdout.on('data', (data) => {
        output += data.toString();
      });

      // Lidar com o término do processo C#
      process.on('close', (code) => {
        if (code === 0) {
          // Processo concluído com sucesso
          try {
            // Converter a saída JSON para um objeto
            const diagramData = JSON.parse(output);

            // Criar a tela do diagrama (implemente a lógica para criar a tela)
            createDiagramScreen(diagramData);

          } catch (error) {
            // Erro ao analisar a saída JSON
            vscode.window.showErrorMessage('Erro ao analisar os dados do diagrama.');
          }
        } else {
          // Erro ao executar o processo C#
          vscode.window.showErrorMessage('Erro ao executar o processo C#.');
        }
      });
    } else {
      // Mostrar mensagem de erro se projectPath for undefined
      vscode.window.showErrorMessage('Nenhum projeto aberto.');
    }
  });

  context.subscriptions.push(disposable);
}

export function deactivate() {}

// Função para criar a tela do diagrama (implemente a lógica para criar a tela)
function createDiagramScreen(diagramData: any) {
  // ...
}
