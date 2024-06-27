$(document).ready(function () {
    var token = 'd0d7cce1-3125-11ef-8e53-0a00184fe694'; // Thay thế 'YOUR_TOKEN_HERE' bằng token của bạn.

    // Hàm xử lý lỗi
    function handleError(xhr, status, error) {
        console.error("Error: " + error);
        console.error("Status: " + status);
        console.error(xhr);
    }

    // Hàm hiển thị spinner
    function showLoader(loaderId) {
        $(loaderId).show();
    }

    // Hàm ẩn spinner
    function hideLoader(loaderId) {
        $(loaderId).hide();
    }

    // Hàm load Tỉnh/Thành
    async function loadTinhThanh() {
        showLoader('#tinhThanhLoader');
        try {
            const response = await $.ajax({
                url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province',
                method: 'GET',
                headers: {
                    'Token': token
                }
            });
            const tinhThanhSelect = $('#btn_TinhThanh');
            response.data.forEach(function (item) {
                tinhThanhSelect.append(new Option(item.ProvinceName, item.ProvinceID));
            });
        } catch (error) {
            handleError(error);
        } finally {
            hideLoader('#tinhThanhLoader');
        }
    }

    // Hàm load Quận/Huyện
    async function loadQuanHuyen(provinceId) {
        showLoader('#quanHuyenLoader');
        try {
            const response = await $.ajax({
                url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district',
                method: 'GET',
                headers: {
                    'Token': token
                }
            });
            const quanHuyenSelect = $('#btn_QuanHuyen');
            response.data.forEach(function (item) {
                if (item.ProvinceID == provinceId) {
                    quanHuyenSelect.append(new Option(item.DistrictName, item.DistrictID));
                }
            });
        } catch (error) {
            handleError(error);
        } finally {
            hideLoader('#quanHuyenLoader');
        }
    }

    // Hàm load Xã/Phường
    async function loadXaPhuong(districtId) {
        showLoader('#xaPhuongLoader');
        try {
            const response = await $.ajax({
                url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward',
                method: 'GET',
                headers: {
                    'Token': token
                },
                data: { district_id: districtId }
            });
            const xaPhuongSelect = $('#btn_XaPhuong');
            response.data.forEach(function (item) {
                xaPhuongSelect.append(new Option(item.WardName, item.WardCode));
            });
        } catch (error) {
            handleError(error);
        } finally {
            hideLoader('#xaPhuongLoader');
        }
    }
   
    // Load Tỉnh/Thành khi trang web được chạy
    loadTinhThanh();

    // Load Quận/Huyện khi Tỉnh/Thành thay đổi
    $('#btn_TinhThanh').change(function () {
        const provinceId = $(this).val();
        const quanHuyenSelect = $('#btn_QuanHuyen');
        const xaPhuongSelect = $('#btn_XaPhuong');

        quanHuyenSelect.empty().append(new Option("Chọn Quận/Huyện", "")).prop('disabled', !provinceId);
        xaPhuongSelect.empty().append(new Option("Chọn Xã/Phường", "")).prop('disabled', true);

        if (provinceId) {
            loadQuanHuyen(provinceId);
        }
    });

    // Load Xã/Phường khi Quận/Huyện thay đổi
    $('#btn_QuanHuyen').change(function () {
        const districtId = $(this).val();
        const xaPhuongSelect = $('#btn_XaPhuong');

        xaPhuongSelect.empty().append(new Option("Chọn Xã/Phường", "")).prop('disabled', !districtId);

        if (districtId) {
            loadXaPhuong(districtId);
           
        }
    }); 
    // Load phí vận chuyển khi Xã/Phường thay đổi

 
  
});