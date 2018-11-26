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

    .controller("ChartController", ["MyService","$scope", ChartController]);

function ChartController(MyService, $scope) {
    var vm = this;

    //$scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
    //$scope.series = ['Doanh SỐ', 'Lợi nhuận'];

    //$scope.data = [
    //    [65, 59, 80, 81, 56, 55, 40],
    //    [28, 48, 40, 19, 86, 27, 90]
    //];


        $scope.labels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
        $scope.data = [300, 500, 100];


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
