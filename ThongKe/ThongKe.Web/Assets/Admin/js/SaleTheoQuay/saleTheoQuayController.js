
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

var homeconfig = {
    pageSize: 10,
    pageIndex: 1
};

var saleTheoQuayController = {
    init: function () {
        // saleTheoQuayController.LoadData();
        //var cn = '@Request.RequestContext.HttpContext.Session["chinhanh"]';
        saleTheoQuayController.loadDdlDaiLyByChiNhanh();
        saleTheoQuayController.registerEvent();
    },

    registerEvent: function () {

        //$('.modal-dialog').draggable();

        $('#frmSearch').validate({
            rules: {
                tungay: {
                    required: true,
                    dateFormat: true
                    //date: true
                },
                denngay: {
                    required: true,
                    dateFormat: true
                    //date: true
                }
            },
            messages: {
                tungay: {
                    required: "Trường này không được để trống."
                    //date: "Chưa đúng định dạng."
                },
                denngay: {
                    required: "Trường này không được để trống."
                    //number: "password phải là số",
                    //date: "Chưa đúng định dạng."
                }
            }
        });


        $('#btnExport').off('click').on('click', function () {
            $('#frmSearch').submit();
        });

        $('#btnReset').off('click').on('click', function () {
            saleTheoQuayController.resetForm();
        });

        $('#btnSearch').off('click').on('click', function () {
            if ($('#frmSearch').valid()) {
                saleTheoQuayController.LoadData();
            }
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
    

    loadDdlDaiLyByChiNhanh: function () {
        var cn = $('#hidCn').data('cn');
        $('#ddlDaiLy').html('');
        var option = '';
        option = '<option value=" ">' + "Tất cả" + '</option>';
        $.ajax({
            url: '/account/GetDmdailyByChiNhanh',
            type: 'GET',
            data: {
                chinhanh: cn
            },
            dataType: 'json',
            success: function (response) {

                var data = JSON.parse(response.data);
                //console.log(data);

                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.Daily + '">' + item.Daily + '</option>'; //chinhanh1

                });
                $('#ddlDaiLy').html(option);

            }
        });
        //homeController.resetForm();
        //var id = $('.btn-edit').data('id');

    },

    LoadData: function (changePageSize) {
        //var name = $('#txtNameS').val();
        //var status = $('#ddlStatusS').val();
        var tungay = $('#txtTuNgay').val();
        var denngay = $('#txtDenNgay').val();
        var daily = $('#ddlDaiLy').val();
        var hidCn = $('#hidCn').data('cn');
        
        //var hidCn = '' ? khoi = $('#ddlKhoi').val() : khoi = $('#hidKhoi').data('khoi');
        if (hidCn == "")
            var khoi = $('#ddlKhoi').val();
        else {
            var khoi = $('#hidKhoi').data('khoi');
        }

        $.ajax({
            url: '/BaoCao/LoadDataSaleTheoQuay',
            type: 'GET',
            data: {
                tungay: tungay,
                denngay: denngay,
                daily: daily,
                cn: hidCn,
                khoi: khoi,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                //console.log(response.data);
                if (response.status) {
                    //console.log(response.data);
                    var data = response.data;
                    //var data = JSON.parse(response.data);

                    //alert(data);
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        //usage:
                        //var formattedDate = $.formattedDate(new Date(parseInt(item.ngaysinh.substr(6))));
                        //alert(formattedDate)
                        var ns = "";
                        if (item.ngaysinh == null)
                            ns = "";
                        else
                            ns = $.formattedDate(new Date(parseInt(item.ngaysinh.substr(6))));

                        html += Mustache.render(template, {
                            stt: item.stt,
                            nguoixuatve: item.nguoixuatve,
                            doanhso: numeral(item.doanhso).format('0,0'),
                            thucthu: numeral(item.thucthu).format('0,0')
                            //tongcong: item.chinhanh,
                            //numeral(item.Product.Price).format('0,0'),
                        });

                    })

                    $('#tblData').html(html);
                    saleTheoQuayController.paging(response.total, function () {
                        saleTheoQuayController.LoadData();
                    }, changePageSize);
                    //saleTheoQuayController.registerEvent();
                }
            }
        })
    },

    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);//lam tron len

        //unbind pagination if it existed or click change size ==> reset
        if (('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData('twbsPagination');
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "trước",
            visiblePages: 10, // tong so trang hien thi , ...12345678910...
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;//khi chuyen trang thi set lai page index
                setTimeout(callback, 200);
            }
        });
    }

}
saleTheoQuayController.init();