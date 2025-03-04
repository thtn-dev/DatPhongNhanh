import { defineConfig, UserConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import fs from 'fs';
import { globSync } from 'glob';
import path from 'path';
import { spawn } from 'child_process';


const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ''
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;
const certificateName = process.env.npm_package_name;


// Define certificate filepath
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
// Define key filepath
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Pattern for CSS files
const cssPattern = /\.css$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

export default defineConfig(async () => {
  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    // Wait for the certificate to be generated
    await new Promise<void>((resolve) => {
      spawn(
        'dotnet',
        [
          'dev-certs',
          'https',
          '--export-path',
          certFilePath,
          '--format',
          'Pem',
          '--no-password'
        ],
        { stdio: 'inherit' }
      ).on('exit', (code) => {
        resolve();
        if (code) {
          process.exit(code);
        }
      });
    });
  }

  const config: UserConfig = {
    base: '/dist/',
    appType: 'spa',
    root: '.',
    // appType: 'custom',
    // root: 'src',
    publicDir: 'public',
    build: {
      manifest: true,
      emptyOutDir: true,
      outDir: '../../wwwroot/dist',
      assetsDir: '',
      rollupOptions: {
        input: [
          ...globSync('src/styles/**/*.css'),
          'src/main.tsx',
        ],
        output: {
          hashCharacters: 'base64',
          // Save entry files to the appropriate folder
          entryFileNames: 'js/[name].[hash].js',
          chunkFileNames: 'js/[name]-chunk.js',
          // Save asset files to the appropriate folder
          assetFileNames: (info) => {
            if (info.name) {
              // If the file is a CSS file, save it to the css folder
              if (cssPattern.test(info.name)) {
                return 'css/[name].[hash][extname]';
              }
              // If the file is an image file, save it to the images folder
              if (imagePattern.test(info.name)) {
                return 'images/[name].[hash][extname]';
              }

              // If the file is any other type of file, save it to the assets folder
              return 'assets/[name][hash][extname]';
            } else {
              // If the file name is not specified, save it to the output directory
              return '[name].[hash][extname]';
            }
          }
        }
      }
    },
    server: {
      strictPort: true,
      https: {
        cert: certFilePath,
        key: keyFilePath
      }
    },
    plugins: [react()],
    optimizeDeps: {
      include: []
    },
    css: {
      preprocessorOptions: {
        scss: {
          api: 'modern-compiler'
        }
      }
    },
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src')
      }
    }
  };

  return config;
});