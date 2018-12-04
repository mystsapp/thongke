$.validator.addMethod("dateFormat",
    function (value, element) {
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split('/');
            var dd = parseInt(adata[0], 10);
            var mm = parseInt(adata[1], 10);
            var yyyy = parseInt(adata[2], 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd)) {
                check = true;
            }
            else {
                check = false;
            }
        } else {
            check = false;
        }
        return this.optional(element) || check;
    },
    "Chưa đúng định dạng dd/mm/yyyy.");


var tuyenTheoNgayDiController = {
    init: function () {
        // tuyenTheoNgayDiController.LoadData();
        tuyenTheoNgayDiController.loadDdlChiNhanh();
        tuyenTheoNgayDiController.registerEvent();
    },

    registerEvent: function () {

        $('#frmSearch').validate({
            rules: {
                tungay: {
                    required: true,
                    //date: true
                    dateFormat: true
                },
                denngay: {
                    required: true,
                    //date: true
                    dateFormat: true
                }
            },
            messages: {
                tungay: {
                    required: "Trường này không được để trống.",
                    date: "Chưa đúng định dạng."
                },
                denngay: {
                    required: "Trường này không được để trống.",
                    //number: "password phải là số",
                    date: "Chưa đúng định dạng."
                }
            }
        });


        $('#btnSearch').off('click').on('click', function () {
            $('#frmSearch').submit();
        });

        $('#btnReset').off('click').on('click', function () {
            tuyenTheoNgayDiController.resetForm();
        });

        $("#txtTuNgay, #txtDenNgay").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd/mm/yy"

        });
    },
    resetForm: function () {
        $('#txtTuNgay').val('');
        $('#txtDenNgay').val('');
    },
    loadDdlChiNhanh: function () {
        $('#ddlChiNhanh').html('');
        var option = '';
        // option = option + '<option value=select>Select</option>';

        $.ajax({
            url: '/account/GetAllChiNhanh',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
               
                var data = JSON.parse(response.data);
                $('#ddlChiNhanh').html('');
               
                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.chinhanh1 + '">' + item.chinhanh1 + '</option>'; //chinhanh1

                });
                $('#ddlChiNhanh').html(option);
               
            }
        });
       
    }

}
tuyenTheoNgayDiController.init();