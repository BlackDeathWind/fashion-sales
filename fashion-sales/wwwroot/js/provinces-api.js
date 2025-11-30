// API helper cho Vietnam Provinces API
// https://provinces.open-api.vn/

const ProvincesAPI = {
    baseUrl: 'https://provinces.open-api.vn/api/v1/',

    async getProvinces() {
        try {
            const response = await fetch(`${this.baseUrl}?depth=1`);
            if (!response.ok) throw new Error('Failed to fetch provinces');
            return await response.json();
        } catch (error) {
            console.error('Error fetching provinces:', error);
            return [];
        }
    },

    async getDistricts(provinceCode) {
        try {
            const response = await fetch(`${this.baseUrl}p/${provinceCode}?depth=2`);
            if (!response.ok) throw new Error('Failed to fetch districts');
            const data = await response.json();
            return data.districts || [];
        } catch (error) {
            console.error('Error fetching districts:', error);
            return [];
        }
    },

    async getWards(districtCode) {
        try {
            const response = await fetch(`${this.baseUrl}d/${districtCode}?depth=2`);
            if (!response.ok) throw new Error('Failed to fetch wards');
            const data = await response.json();
            return data.wards || [];
        } catch (error) {
            console.error('Error fetching wards:', error);
            return [];
        }
    },

    populateDatalist(datalist, items) {
        if (!datalist) return;
        datalist.innerHTML = '';
        items.forEach(item => {
            const option = document.createElement('option');
            option.value = item.name;
            option.dataset.code = item.code;
            datalist.appendChild(option);
        });
    }
};

function initAddressPicker(config = {}) {
    const {
        provinceInputId,
        provinceListId,
        provinceCodeId,
        provinceNameId,
        districtInputId,
        districtListId,
        districtCodeId,
        districtNameId,
        wardInputId,
        wardListId,
        wardCodeId,
        wardNameId,
        initialProvinceCode = null,
        initialDistrictCode = null,
        initialWardCode = null,
        onChange = null
    } = config;

    const provinceInput = document.getElementById(provinceInputId);
    const provinceList = document.getElementById(provinceListId);
    const provinceCodeInput = document.getElementById(provinceCodeId);
    const provinceNameInput = document.getElementById(provinceNameId);

    const districtInput = document.getElementById(districtInputId);
    const districtList = document.getElementById(districtListId);
    const districtCodeInput = document.getElementById(districtCodeId);
    const districtNameInput = document.getElementById(districtNameId);

    const wardInput = document.getElementById(wardInputId);
    const wardList = document.getElementById(wardListId);
    const wardCodeInput = document.getElementById(wardCodeId);
    const wardNameInput = document.getElementById(wardNameId);

    if (!provinceInput || !provinceList || !provinceCodeInput || !districtInput || !districtList || !districtCodeInput || !wardInput || !wardList || !wardCodeInput) {
        console.warn('Address picker: missing required elements');
        return;
    }

    const notifyChange = () => {
        if (typeof onChange === 'function') {
            onChange({
                provinceCode: provinceCodeInput.value,
                provinceName: provinceNameInput?.value,
                districtCode: districtCodeInput.value,
                districtName: districtNameInput?.value,
                wardCode: wardCodeInput.value,
                wardName: wardNameInput?.value,
                streetAddress: null
            });
        }
    };

    const disableInput = (input, placeholder) => {
        if (!input) return;
        input.value = '';
        input.placeholder = placeholder;
        input.disabled = true;
    };

    const enableInput = (input, placeholder) => {
        if (!input) return;
        input.disabled = false;
        if (placeholder) {
            input.placeholder = placeholder;
        }
    };

    const findByName = (items, value) => {
        if (!value) return null;
        const normalized = value.trim().toLowerCase();
        return items.find(item => item.name.toLowerCase() === normalized);
    };

    const clearProvince = (trigger = true) => {
        provinceCodeInput.value = '';
        if (provinceNameInput) provinceNameInput.value = '';
        districtInput.value = '';
        wardInput.value = '';
        disableInput(districtInput, 'Chọn Tỉnh/Thành phố trước');
        disableInput(wardInput, 'Chọn Quận/Huyện trước');
        districtList.innerHTML = '';
        wardList.innerHTML = '';
        districtCodeInput.value = '';
        districtNameInput && (districtNameInput.value = '');
        wardCodeInput.value = '';
        wardNameInput && (wardNameInput.value = '');
        if (trigger) notifyChange();
    };

    const clearDistrict = (trigger = true) => {
        districtCodeInput.value = '';
        if (districtNameInput) districtNameInput.value = '';
        districtInput.value = '';
        disableInput(wardInput, 'Chọn Quận/Huyện trước');
        wardList.innerHTML = '';
        wardCodeInput.value = '';
        wardNameInput && (wardNameInput.value = '');
        if (trigger) notifyChange();
    };

    const clearWard = (trigger = true) => {
        wardCodeInput.value = '';
        if (wardNameInput) wardNameInput.value = '';
        wardInput.value = '';
        if (trigger) notifyChange();
    };

    let provinces = [];
    let districts = [];
    let wards = [];

    disableInput(districtInput, 'Chọn Tỉnh/Thành phố trước');
    disableInput(wardInput, 'Chọn Quận/Huyện trước');
    provinceInput.placeholder = 'Nhập để tìm Tỉnh/Thành phố';
    provinceInput.disabled = true;

    const loadDistricts = async (provinceCode, selectedDistrictCode = null, selectedWardCode = null) => {
        if (!provinceCode) return;
        districtInput.value = '';
        wardInput.value = '';
        disableInput(districtInput, 'Đang tải...');
        disableInput(wardInput, 'Chọn Quận/Huyện trước');
        districts = await ProvincesAPI.getDistricts(provinceCode);
        ProvincesAPI.populateDatalist(districtList, districts);
        enableInput(districtInput, 'Nhập để tìm Quận/Huyện');

        if (selectedDistrictCode) {
            const selectedDistrict = districts.find(d => String(d.code) === String(selectedDistrictCode));
            if (selectedDistrict) {
                setDistrict(selectedDistrict, false, selectedWardCode);
            }
        } else {
            clearDistrict(false);
        }
    };

    const loadWards = async (districtCode, selectedWardCode = null) => {
        if (!districtCode) return;
        wardInput.value = '';
        disableInput(wardInput, 'Đang tải...');
        wards = await ProvincesAPI.getWards(districtCode);
        ProvincesAPI.populateDatalist(wardList, wards);
        enableInput(wardInput, 'Nhập để tìm Phường/Xã');

        if (selectedWardCode) {
            const selectedWard = wards.find(w => String(w.code) === String(selectedWardCode));
            if (selectedWard) {
                setWard(selectedWard, false);
            }
        } else {
            clearWard(false);
        }
    };

    const setProvince = (province, trigger = true, preselectDistrict = null, preselectWard = null) => {
        if (!province) return;
        provinceInput.value = province.name;
        provinceCodeInput.value = province.code;
        if (provinceNameInput) provinceNameInput.value = province.name;
        loadDistricts(province.code, preselectDistrict, preselectWard);
        if (trigger) notifyChange();
    };

    const setDistrict = (district, trigger = true, preselectWard = null) => {
        if (!district) return;
        districtInput.value = district.name;
        districtCodeInput.value = district.code;
        if (districtNameInput) districtNameInput.value = district.name;
        enableInput(wardInput, 'Nhập để tìm Phường/Xã');
        loadWards(district.code, preselectWard);
        if (trigger) notifyChange();
    };

    const setWard = (ward, trigger = true) => {
        if (!ward) return;
        wardInput.value = ward.name;
        wardCodeInput.value = ward.code;
        if (wardNameInput) wardNameInput.value = ward.name;
        if (trigger) notifyChange();
    };

    ProvincesAPI.getProvinces().then(data => {
        provinces = data;
        ProvincesAPI.populateDatalist(provinceList, provinces);
        enableInput(provinceInput, 'Nhập để tìm Tỉnh/Thành phố');

        if (initialProvinceCode) {
            const selectedProvince = provinces.find(p => String(p.code) === String(initialProvinceCode));
            if (selectedProvince) {
                setProvince(selectedProvince, false, initialDistrictCode, initialWardCode);
            }
        }
    });

    const handleProvinceChange = value => {
        const selected = findByName(provinces, value);
        if (selected) {
            setProvince(selected);
        } else {
            clearProvince();
        }
    };

    const handleDistrictChange = value => {
        const selected = findByName(districts, value);
        if (selected) {
            setDistrict(selected);
        } else {
            clearDistrict();
        }
    };

    const handleWardChange = value => {
        const selected = findByName(wards, value);
        if (selected) {
            setWard(selected);
        } else {
            clearWard();
        }
    };

    provinceInput.addEventListener('change', () => handleProvinceChange(provinceInput.value));
    provinceInput.addEventListener('blur', () => handleProvinceChange(provinceInput.value));

    districtInput.addEventListener('change', () => handleDistrictChange(districtInput.value));
    districtInput.addEventListener('blur', () => handleDistrictChange(districtInput.value));

    wardInput.addEventListener('change', () => handleWardChange(wardInput.value));
    wardInput.addEventListener('blur', () => handleWardChange(wardInput.value));
}

window.initAddressPicker = initAddressPicker;