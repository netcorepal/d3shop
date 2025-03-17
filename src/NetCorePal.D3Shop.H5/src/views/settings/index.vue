<template>
  <van-popup
    v-model:show="show"
    position="right"
    :style="{ width: '100%', height: '100%' }"
  >
    <div class="settings">
      <van-nav-bar
        :title="t('settings.title')"
        left-arrow
        @click-left="onClose"
      />

      <van-cell-group class="settings-group">
        <van-cell :title="t('settings.language')" is-link>
          <template #value>
            <LanguageSwitcher />
          </template>
        </van-cell>

        <van-cell :title="t('settings.theme')" is-link>
          <template #value>
            <ThemeColorPicker v-model="currentThemeColor" />
          </template>
        </van-cell>

        <van-cell :title="t('settings.appearance')" :border="false" />
        <van-radio-group v-model="themeMode" class="theme-radio-group">
          <van-cell-group inset>
            <van-cell clickable @click="themeMode = 'light'">
              <template #title>
                <span class="radio-label">{{ t('settings.lightMode') }}</span>
              </template>
              <template #right-icon>
                <van-radio name="light" />
              </template>
            </van-cell>
            <van-cell clickable @click="themeMode = 'dark'">
              <template #title>
                <span class="radio-label">{{ t('settings.darkMode') }}</span>
              </template>
              <template #right-icon>
                <van-radio name="dark" />
              </template>
            </van-cell>
            <van-cell clickable @click="themeMode = 'system'">
              <template #title>
                <span class="radio-label">{{ t('settings.systemMode') }}</span>
              </template>
              <template #right-icon>
                <van-radio name="system" />
              </template>
            </van-cell>
          </van-cell-group>
        </van-radio-group>

        <van-cell :title="t('settings.currentEnvironment')">
          <template #value>
            <span class="environment-text">{{ environmentName }}</span>
          </template>
        </van-cell>

        <van-cell :title="t('settings.version')">
          <template #value>
            <span class="version-text">v1.0.0</span>
          </template>
        </van-cell>
      </van-cell-group>
    </div>
  </van-popup>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useAppStore } from '@/store/app';
import LanguageSwitcher from '@/components/LanguageSwitcher.vue';
import ThemeColorPicker from '@/components/ThemeColorPicker.vue';
import { useI18n } from 'vue-i18n';

const { t } = useI18n();
const router = useRouter();
const appStore = useAppStore();
const show = ref(false);
// 主题模式状态
const themeMode = ref(
  appStore.followSystem ? 'system' : (appStore.themeVars.dark ? 'dark' : 'light')
);


// 监听主题模式变化
watch(themeMode, (newMode) => {
  switch (newMode) {
    case 'light':
      appStore.setDarkMode(false);
      break;
    case 'dark':
      appStore.setDarkMode(true);
      break;
    case 'system':
      appStore.followSystemTheme();
      break;
  }
});

// 主题色相关
const currentThemeColor = ref(localStorage.getItem('theme-color') || '#1989fa');

// 监听主题色变化
watch(currentThemeColor, (newColor) => {
  localStorage.setItem('theme-color', newColor);
  const lighterColor = adjustColor(newColor, 0.9);
  
  appStore.updateThemeVars({
    primaryColor: newColor,
    primaryColorLight: lighterColor
  });
});

// 调整颜色透明度的辅助函数
const adjustColor = (color: string, opacity: number) => {
  if (color.startsWith('#')) {
    const r = parseInt(color.slice(1, 3), 16);
    const g = parseInt(color.slice(3, 5), 16);
    const b = parseInt(color.slice(5, 7), 16);
    return `rgba(${r}, ${g}, ${b}, ${opacity})`;
  }
  return color;
};

const environmentName = import.meta.env.VITE_NetCorePalD3Shop_EnvironmentName;

const onClose = () => {
  router.back();
};

onMounted(() => {
  show.value = true;
  // 初始化主题色
  const lighterColor = adjustColor(currentThemeColor.value, 0.9);
  appStore.updateThemeVars({
    primaryColor: currentThemeColor.value,
    primaryColorLight: lighterColor
  });
});
</script>

<style scoped>
.settings {
  height: 100%;
  display: flex;
  flex-direction: column;
  background: var(--van-background);
}

.settings-group {
  margin-top: var(--van-padding-sm);
}

:deep(.van-nav-bar) {
  flex-shrink: 0;
}

:deep(.van-cell) {
  align-items: center;
}

:deep(.van-cell__title) {
  flex: 3;
}

:deep(.van-cell__value) {
  flex: 2;
  text-align: right;
  overflow: visible;
}

:deep(.van-radio__label) {
  color: inherit;
}

.theme-radio-group {
  margin: 0 16px 16px;
}

.radio-label {
  font-size: 14px;
}

.environment-text,
.version-text {
  color: var(--van-text-color-2);
}

/* 系统深色模式 */
@media (prefers-color-scheme: dark) {
  :deep(.settings) {
    background: #121212;
  }

  :deep(.van-popup) {
    background: #121212;
  }

  :deep(.van-nav-bar) {
    background: #121212;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  }

  :deep(.van-nav-bar__title),
  :deep(.van-nav-bar__text),
  :deep(.van-nav-bar .van-icon) {
    color: rgba(255, 255, 255, 0.9);
  }

  :deep(.van-cell) {
    background: #1a1a1a;
    color: rgba(255, 255, 255, 0.9);
  }

  :deep(.van-cell__title),
  :deep(.van-cell__value),
  :deep(.radio-label) {
    color: rgba(255, 255, 255, 0.9);
  }

  :deep(.van-cell-group--inset) {
    background: #121212;
  }

  :deep(.van-cell-group--inset .van-cell) {
    background: #1a1a1a;
  }

  :deep(.environment-text),
  :deep(.version-text) {
    color: rgba(255, 255, 255, 0.7);
  }
}

/* 手动设置深色模式 */
:deep([data-theme='dark']) .settings,
:deep([data-theme='dark']) .van-popup {
  background: #121212;
}

:deep([data-theme='dark']) .van-nav-bar {
  background: #121212;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

:deep([data-theme='dark']) .van-nav-bar__title,
:deep([data-theme='dark']) .van-nav-bar__text,
:deep([data-theme='dark']) .van-nav-bar .van-icon {
  color: rgba(255, 255, 255, 0.9);
}

:deep([data-theme='dark']) .van-cell {
  background: #1a1a1a;
  color: rgba(255, 255, 255, 0.9);
}

:deep([data-theme='dark']) .van-cell__title,
:deep([data-theme='dark']) .van-cell__value,
:deep([data-theme='dark']) .radio-label {
  color: rgba(255, 255, 255, 0.9);
}

:deep([data-theme='dark']) .van-cell-group--inset {
  background: #121212;
}

:deep([data-theme='dark']) .van-cell-group--inset .van-cell {
  background: #1a1a1a;
}

:deep([data-theme='dark']) .environment-text,
:deep([data-theme='dark']) .version-text {
  color: rgba(255, 255, 255, 0.7);
}
</style>
