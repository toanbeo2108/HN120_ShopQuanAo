$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    } 
    var Parameter = {
        url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
        method: "GET",
        responseType: "application/json",
    };
    var promise = axios(Parameter);
    promise.then(function (result) {
        renderCity(result.data);
    });


    // lấy tiền ship gửi về controller
    $('body').on('click', '#dat-hang', function () {
        var tongtiendonhang = 0;
        var tienship = $('#btn_PhiShip').val().replace(/[^0-9]/g, '');;
        var tinh = $('#city').val();
        var huyen = $('#district').val();
        var xa = $('#ward').val();
        var cuthe = $('#street').val();
        var tongtienhang = $('#tong-tien-hang').text().replace(/[^0-9]/g, '');
        var tiengiam = $('#btn_giamgia').val();
        tongtiendonhang = parseFloat(tongtienhang) + parseFloat(tienship) - parseFloat(tiengiam);



        console.log(tongtiendonhang);

        if (tongtiendonhang != 0) {
            $.ajax({
                url: '/CustomerHome/DatHang',
                type: 'POST',
                data: {
                    tienship: tienship,
                    tinh: tinh,
                    huyen: huyen,
                    xa: xa,
                    cuthe: cuthe,
                    tongtiendonhang: tongtiendonhang,
                    tiengiam: tiengiam
                }

                ,
                success: function (response) {
                    localStorage.setItem('notification', JSON.stringify({ message: 'Thanh toán thành công', type: 'success' }));

                    location.reload(true);

                },
                error: function (xhr, status, error) {
                    localStorage.setItem('notification', JSON.stringify({ message: 'Thanh toán thất bại', type: 'error' }));
                    location.reload(true);

                }
            });

            localStorage.setItem('notification', JSON.stringify({ message: 'Thanh toán thành công', type: 'success' }));
            location.reload(true);

        }
        else {
            $.ajax({
                url: '/Customer/CustomerHome/GianHangNguoiDung',
                type: 'GET',
                success: function (data) {
                    // Xử lý dữ liệu trả về tại đây
                    $('#target-element').html(data); // Thay '#target-element' bằng ID của phần tử nơi bạn muốn chèn nội dung
                },
                error: function (xhr, status, error) {
                    console.error('Có lỗi xảy ra: ' + error);
                }
            });
            location.reload(true);

        }
        
    });





    //format tiền
    var moneyElements = document.getElementsByName('tien');
    $.each(moneyElements, function (index, element) {
        var $element = $(element);
        var amountText = $element.text().trim(); // Lấy số tiền từ văn bản và loại bỏ khoảng trắng

        // Xử lý số tiền từ văn bản
        var amountWithoutCommas = amountText.split(',')[0].replace(/,/g, ''); // Bỏ dấu phẩy và phần sau dấu phẩy
        var amount = parseFloat(amountWithoutCommas);

        // Định dạng số tiền và gán lại vào văn bản nếu số tiền hợp lệ
        if (!isNaN(amount)) {
            $element.text(formatMoney(amount));
        }
    });

    



    var token = 'd01771f0-3f8b-11ef-8f55-4ee3d82283af';
    async function loadTinhThanh() {
        var tinhthanh = $('#city').val();
        let provinceID;
        let districtID;
        let wardCode;
        try {
            const response = await $.ajax({
                url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province',
                method: 'GET',
                headers: {
                    'Token': token
                }
            });

            response.data.forEach(function (item) {
                if (item.ProvinceName.trim() === tinhthanh.replace(/^Tỉnh\s+|\s+$/g, '').trim()) { // So sánh chuỗi đã loại bỏ khoảng trắng thừa và dấu tiếng Việt
                    provinceID = item.ProvinceID;
                }
            });
            await loadQuanHuyen(provinceID)
        } catch (error) {
            alert('Không tìm thấy mã tỉnh.');
        }
        async function loadQuanHuyen(provinceId) {
            var quanhuyen = $('#district').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district',
                    method: 'GET',
                    headers: {
                        'Token': token
                    }
                });

                response.data.forEach(function (item) {
                    if (item.DistrictName.trim() === quanhuyen.trim()) {

                        districtID = item.DistrictID;
                    }

                });
                await loadXaPhuong(districtID);
            } catch (error) {
                handleError(error);
            }
        }
        async function loadXaPhuong(districtID) {
            var xaphuong = $('#ward').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward',
                    method: 'GET',
                    headers: {
                        'Token': token
                    },
                    data: { district_id: districtID }
                });

                response.data.forEach(function (item) {
                    if (item.WardName.trim() === xaphuong.trim()) {

                        wardCode = item.WardCode;
                    }
                });
                //if (wardCode == undefined) {
                //  $.notify('Hiện giao hàng nhanh chưa hỗ trợ địa điểm này, thông cảm !', 'error')
                //}
                console.log('Mã Tỉnh' + provinceID + 'Mã quận: ' + districtID + 'Mã xã: ' + wardCode);
                await getShippingFee(provinceID, districtID, wardCode)
            } catch (error) {

            }
        }
        async function getShippingFee(provinceID, districtID, wardCode) {
            const SshopId = 192652;
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Token': token
                        //'ShopId': SshopId
                    },
                    data: JSON.stringify({
                        "service_type_id": 2,
                        "from_district_id": 3440,
                        "from_ward_code": "13009",
                        "to_district_id": districtID,
                        "to_ward_code": wardCode,
                        "height": 20,
                        "length": 30,
                        "weight": 3000,
                        "width": 40,
                        "insurance_value": 0,
                        "coupon": null

                    })
                });

                if (response.code === 200) {

                    $('#btn_PhiShip').val(response.data.total);
                    var tongtiendonhang = 0;
                    var tienship = $('#btn_PhiShip').val().replace(/[^0-9]/g, '');
                    var tongtienhang = $('#tong-tien-hang').text().replace(/[^0-9]/g, '');
                    var tiengiam = $('#btn_giamgia').val()//.replace(/[^0-9]/g, '');

                    tongtiendonhang = parseFloat(tongtienhang) + parseFloat(tienship) - parseFloat(tiengiam);
                    $('#tong-don-hang').val(formatMoney(tongtiendonhang));

                    // Format tiền ship trong input
                    $('#btn_PhiShip').val(formatMoney(response.data.total));

                    $('#btn_giamgia').val(formatMoney(tiengiam));

                } else {
                    console.error('Error:', response.message);
                }
            } catch (error) {
                console.error('Error calling API:', error);
            }
        }
    }

    $('#ward').change(function () {
        loadTinhThanh();


    })



})
function renderCity(data) {
    var citis = document.getElementById("city");
    var districts = document.getElementById("district");
    var wards = document.getElementById("ward");
    for (const x of data) {
        citis.options[citis.options.length] = new Option(x.Name, x.Name);
    }
    citis.onchange = function () {
        districts.length = 1;
        wards.length = 1;
        if (this.value != "") {
            const result = data.filter(n => n.Name === this.value);

            for (const k of result[0].Districts) {
                districts.options[districts.options.length] = new Option(k.Name, k.Name);
            }
        }
    };
    districts.onchange = function () {
        wards.length = 1;
        const dataCity = data.filter((n) => n.Name === citis.value);
        if (this.value != "") {
            const dataWards = dataCity[0].Districts.filter(n => n.Name === this.value)[0].Wards;

            for (const w of dataWards) {
                wards.options[wards.options.length] = new Option(w.Name, w.Name);
            }
        }
    };
    
}
function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}