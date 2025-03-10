export const storage = {
  set<T>(key: string, value: T, ttl: number): void {
    const item: StorageItem<T> = {
      value,
      expire: Date.now() + ttl * 1000
    }
    localStorage.setItem(key, JSON.stringify(item))
  },

  get<T>(key: string): T | null {
    const itemStr = localStorage.getItem(key)
    if (!itemStr) return null

    const item = JSON.parse(itemStr) as StorageItem<T>
    if (Date.now() > item.expire) {
      localStorage.removeItem(key)
      return null
    }
    return item.value
  },
  remove(key: string): void {
    localStorage.removeItem(key)
  }
}