import { defineStore } from 'pinia';

interface CartItem {
  id: number;
  title: string;
  price: string | number;
  thumb: string;
  quantity: number;
}

interface CartState {
  items: CartItem[];
}

export const useCartStore = defineStore('cart', {
  state: (): CartState => ({
    items: JSON.parse(localStorage.getItem('cart-items') || '[]')
  }),

  getters: {
    totalCount: (state) => state.items.reduce((sum, item) => sum + item.quantity, 0),
    totalAmount: (state) => state.items.reduce((sum, item) => sum + Number(item.price) * item.quantity, 0),
    itemCount: (state) => (id: number) => state.items.find(item => item.id === id)?.quantity || 0
  },

  actions: {
    // 添加商品到购物车
    addToCart(item: Omit<CartItem, 'quantity'>) {
      const existingItem = this.items.find(i => i.id === item.id);
      if (existingItem) {
        existingItem.quantity++;
      } else {
        this.items.push({ ...item, quantity: 1 });
      }
      this.saveToStorage();
    },

    // 从购物车移除商品
    removeFromCart(id: number) {
      const index = this.items.findIndex(item => item.id === id);
      if (index > -1) {
        this.items.splice(index, 1);
        this.saveToStorage();
      }
    },

    // 更新商品数量
    updateQuantity(id: number, quantity: number) {
      const item = this.items.find(item => item.id === id);
      if (item) {
        item.quantity = quantity;
        if (quantity <= 0) {
          this.removeFromCart(id);
        } else {
          this.saveToStorage();
        }
      }
    },

    // 清空购物车
    clearCart() {
      this.items = [];
      this.saveToStorage();
    },

    // 保存到本地存储
    saveToStorage() {
      localStorage.setItem('cart-items', JSON.stringify(this.items));
    }
  }
}); 