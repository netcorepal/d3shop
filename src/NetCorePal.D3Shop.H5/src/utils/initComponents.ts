

import { Toast, Dialog, Popup, Popover } from 'vant';
import type { App } from 'vue';

export function setupComponents(app: App) {
  app.use(Toast);
  app.use(Dialog);
  app.use(Popup);
  app.use(Popover);

}