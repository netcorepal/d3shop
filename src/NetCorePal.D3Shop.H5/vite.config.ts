
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import Components from 'unplugin-vue-components/vite';
import { VantResolver } from 'unplugin-vue-components/resolvers';
import path from 'path';
// import { createStyleImportPlugin } from 'vite-plugin-style-import';

export default defineConfig({
  plugins: [
    vue(),
    Components({
      resolvers: [VantResolver()],
    }),
    // createStyleImportPlugin({
    //   libs: [
    //     {
    //       libraryName: 'vant',
    //       esModule: true,
    //       resolveStyle: (name) => `vant/es/${name}/style`,
    //     },
    //   ],
    // }),
  ],
  server: {
    port: 10086
  }, resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
});
