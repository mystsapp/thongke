angular
    .module("app", ["chart.js"])


    .service("MyService", ["$http", function ($http) {
        return {
            getMyData: function () {
                return $http.get("https://gist.githubusercontent.com/dannyjhonston/4fade791505e812eae1a3fea2cde6ac4/raw/d73d8eb12cff8911b6b7c681718a760129ae1299/angular-chart-data.json", null, {
                    responseType: "json"
                });
            }
        };
    }])

    .controller("ChartController", ["MyService", "$scope","$http", ChartController]);
//ChartController.$inject=[]

function ChartController(MyService, $scope,$http) {
    var vm = this;

    $scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
    $scope.series = ['Doanh SỐ', 'Lợi nhuận'];

    $scope.data = [
        [65, 59, 80, 81, 56, 55, 40],
        [28, 48, 40, 19, 86, 27, 90]
    ];


        //$scope.labels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
        //$scope.data = [300, 500, 100];
    //var tungay = '01/01/2016';
    //var denngay = '01/01/2019';
    //var cn = 'STS';
    //var khoi = 'OB';

    $http({
        url: '/Home/LoadDataKhachLeHethong',
        type: 'GET'
        //data: {
        //    tungay: "01/01/2016",
        //    denngay: "01/01/2019",
        //    chinhanh: "STS",
        //    khoi: "OB"
        //}
    }).then(function successCallback(response) {
        // this callback will be called asynchronously
        // when the response is available

        var labels = [];
        var chartData = [];
        var thucthuht = [];
        var thucthucu = [];

        var ajaxdata = response.data;
        //console.log(ajaxdata.data);

        $.each(ajaxdata.data, function (i, item) {
            labels.push(item.dailyxuatve);
            thucthuht.push(numeral(item.thucthuht).format('0.0')); 
            thucthucu.push(numeral(item.thucthucu).format('0.0'));
            //thucthucu.push(String.format("{0:#,##0}"), item.thucthucu);
           // console.log(item.dailyxuatve);
        });

        //for (var i = 0; i < ajaxdata.length; i++) {
        //    // set the key/property (input element) for your object
        //    var ele = ajaxdata[i];
        //    console.log(ele);
        //    //option = option + '<option value="' + ele + '">' + ele + '</option>'; //chinhanh1
        //    // add the property to the object and set the value
        //    //params[ele] = $('#' + ele).val();
        //}

        chartData.push(thucthuht);
        chartData.push(thucthucu);
        $scope.data = chartData;
        $scope.labels = labels;
        console.log(chartData);
        //$scope.data = response.data;
    }, function errorCallback(response) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
        $scope.error = response.statusText;
    });


    //$scope.tabledata = [];

    //$scope.labels = [];
    //$scope.series = ['Doanh SỐ', 'Lợi nhuận'];

    //$scope.chartdata = [];

    //function getStatistic() {
    //    var config = {
    //        params: {
    //            fromDate: '01/01/2016',
    //            toDate: '01/01/2019'
    //        }
    //    }
    //    //apiService.get('api/statistic/getrevenue?fromDate='+config.params.fromDate+"&toDate="+config.params.toDate, null, function (response) {
    //    apiService.get('api/statistic/getrevenue', config, function (response) {
    //        $scope.tabledata = response.data;
    //        var labels = [];
    //        var chartData = [];
    //        var revenues = [];
    //        var benefits = [];
    //        $.each(response.data, function (i, item) {
    //            labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
    //            revenues.push(item.Revenues);
    //            benefits.push(item.Benefit);
    //        });
    //        //console.log(labels);
    //        chartData.push(revenues);
    //        chartData.push(benefits);
    //        //console.log(chartData);
    //        $scope.chartdata = chartData;
    //        $scope.labels = labels;
    //    }, function (response) {
    //        notificationService.displayError('Không thể tải dữ liệu');
    //    });
    //}
    //getStatistic();

    //MyService.getMyData().then(function (response) {

    //    // Data.
    //    vm.barChart = response.data;

    //}, function (response) {
    //    console.log("Error: " + response);
    //});


}
