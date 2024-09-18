const esbuild = require('esbuild');
const vsce = require('@vscode/vsce'); // Importe a biblioteca vsce

const production = process.argv.includes('--production');
const watch = process.argv.includes('--watch');

/**
 * @type {import('esbuild').Plugin}
 */
const esbuildProblemMatcherPlugin = {
  name: 'esbuild-problem-matcher',

  setup(build) {
    build.onStart(() => {
      console.log('[watch] build started');
    });
    build.onEnd((result) => {
      result.errors.forEach(({ text, location }) => {
        console.error(`✘ [ERROR] ${text}`);
        console.error(`    ${location.file}:${location.line}:${location.column}:`);
      });
      console.log('[watch] build finished');
    });
  },
};

async function main() {
  const config = {
    entryPoints: [
      'src/extension.ts'
    ],
    bundle: true,
    format: 'cjs',
    minify: production,
    sourcemap: !production,
    sourcesContent: false,
    platform: 'node',
    outfile: 'out/extension.js', // Defina o caminho de saída para a pasta "out"
    external: ['vscode'],
    logLevel: 'silent',
    plugins: [esbuildProblemMatcherPlugin]
  };

  if (watch) {
    const ctx = await esbuild.context(config);
    await ctx.watch();
  } else {
    await esbuild.build(config);
    if (production) {
      try {
        await vsce.createVSIX({
          cwd: process.cwd(),
          packagePath: 'out/lowcodecsharp-extension-0.0.1.vsix', // Defina o nome do arquivo VSIX
        });
        console.log('VSIX package created successfully.');
      } catch (error) {
        console.error('Error creating VSIX package:', error);
      }
    }
  }
}

main().catch(e => {
  console.error(e);
  process.exit(1);
});
