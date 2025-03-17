<template>
  <div class="address">
    <van-address-list v-model="chosenAddressId" :list="addressList" default-tag-text="默认" :switchable="true"
      
      @add="showAddAddressForm" @edit="editAddress" />
    <van-popup v-model:show="show" position="right" :style="{ width: '90%', height: '100%' }">
      <van-address-edit :area-list="areaList" :show-delete="isEdit" show-set-default :address-info="editAddressItem"
        :area-columns-placeholder="['请选择', '请选择', '请选择']" @save="onSave" @delete="onDelete" />
    </van-popup>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { getDeliveryAddresses, addDeliveryAddress, updateDeliveryAddress, deleteDeliveryAddress } from '@/api/clientUser';
import { showToast } from 'vant';
import type { ClientUserDeliveryAddressInfo } from '@/api/model/clientUser';
import type { AddressListAddress } from 'vant';
import { areaList } from '@vant/area-data';
import type { AddressEditInfo } from 'vant';

const addresses = ref<ClientUserDeliveryAddressInfo[]>([]);
const addressList = ref<AddressListAddress[]>([]);
const loading = ref(false);
const finished = ref(false);
const show = ref(false);
const isEdit = ref(false);
const chosenAddressId = ref('');


const editAddressItem = ref<AddressEditInfo>({
  name: '',
  tel: '',
  province: '',
  city: '',
  county: '',
  addressDetail: '',
  areaCode: '',
  isDefault: false
});



const fetchAddresses = async () => {
  loading.value = true;
  try {
    const response = await getDeliveryAddresses();
    console.log(response, '获取地址响应')
    addresses.value = response;
    addressList.value = response.map(address => ({
      id: address.id,
      name: address.recipientName,
      tel: address.phone,
      address: address.address,
      isDefault: address.isDefault
    }));
    finished.value = true;
  } catch (error) {
    showToast('获取地址失败');
  } finally {
    loading.value = false;
  }
};

const showAddAddressForm = () => {
  show.value = true;
  editAddressItem.value = {
    name: '',
    tel: '',
    province: '',
    city: '',
    county: '',
    addressDetail: '',
    areaCode: ''
  };
};



const onSave = async (address: AddressEditInfo) => {
  if (isEdit.value) {
    try {
      await updateDeliveryAddress({
        deliveryAddressId: chosenAddressId.value,
        recipientName: address.name,
        phone: address.tel,
        address: address.province + address.city + address.county + address.addressDetail,
        setAsDefault: address.isDefault ?? false
      });
      showToast('地址更新成功');
      fetchAddresses();
      show.value = false;
      isEdit.value = false;
    }
    catch (error) {
      showToast('更新地址失败');
      console.error(error);
    }
  }
  else {
    try {
      await addDeliveryAddress({
        recipientName: address.name,
        phone: address.tel,
        address: address.province + address.city + address.county + address.addressDetail,
        setAsDefault: address.isDefault ?? false
      });
      showToast('地址添加成功');
      fetchAddresses();
      show.value = false;
    }
    catch (error) {
      showToast('添加地址失败');
      console.error(error);
    }
  }
};

const onDelete = async () => {

  try {
    await deleteDeliveryAddress(chosenAddressId.value);
    showToast('地址删除成功');
    fetchAddresses();
    show.value = false;
  }
  catch (error) {
    showToast('删除地址失败');
    console.error(error);
  };
}

const editAddress = (address: AddressListAddress) => {
  // 编辑地址逻辑
  console.log('编辑地址:', address);
  chosenAddressId.value = address.id.toString();
  isEdit.value = true;
  // TODO: 等待后段完善反显的省市区及编号
  // editAddressItem.value = {
  //   name: address.name,
  //   tel: address.tel.toString(),
  //   province: ad?.province ?? '',
  //   city: ad?.city ?? '',
  //   county: ad?.district ?? '',
  //   addressDetail: ad?.address ?? '',
  //   areaCode: ''
  // };
  show.value = true;

};


// /**
//  * 正则匹配省市区
//  * @param address 
//  */
// function parseAddress(address: string) {
//   const regex = /^(?<province>(北京|上海|天津|重庆)市)(?<city>市辖区|.*?市)(?<district>[^区]+区|[^县]+县)(?<address>.*)$/;
//   const match = address.match(regex);
//   if (!match) return { province: null, city: null, district: null, address: null };

//   // 直辖市特殊处理：强制市级为"市辖区"
//   const groups = match?.groups;
//   // const isMunicipality = ['北京市', '上海市', '天津市', '重庆市'].includes(groups?.province ?? '');
//   return {
//     province: groups?.province ?? '',
//     city: groups?.city ?? groups?.province ?? '',
//     district: groups?.district ?? '',
//     address: groups?.address ?? ''
//   };
// }
onMounted(fetchAddresses);
</script>

<style scoped>
.address {
  padding: 16px;
}
</style>