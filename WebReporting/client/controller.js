angular.module('app').controller('appCtrl', ['$filter', 'dataService', 'setupjson', 'NgTableParams', '$sce', function ($filter, dataService, setupjson, NgTableParams, $sce) {
    var self = this;

    self.application = setupjson.application;
    self.service_url = self.application.serviceurl;
    self.menus = setupjson.environments;
    self.tabs = setupjson.repositories;
    self.env_id = self.menus[0].id; // init
    self.repo_id = self.tabs[0].id; // init
    self.sorting = {}; // init
    self.filters = {}; // init
    self.sorting[self.tabs[0].default_sort_col_name] = self.tabs[0].default_sort_col_order; // init
    self.dataLoaded = false; // init
    self.dataCount = 0; // init
    self.dataMsg = "";

    self.tableParams = new NgTableParams(
        { page: 1, count: 10, sorting: self.sorting, filter: self.filters }, {}
    );
    self.cols = []; // init
    self.date_val = "''";

    self.datatableRefresh = function () {
        self.dataLoaded = false; // start loading spinner
        dataService.getdata(self.service_url, self.env_id, self.repo_id, self.date_val).then(function (res) {
            self.dataLoaded = true; // stop loading spinner and show the datatable
            if (res.env_id == self.env_id && res.repo_id == self.repo_id) {
                self.dataCount = res.total;
                self.dataMsg = res.msg.toLowerCase().contains("success") && self.dataCount == 0 ? "<div>No records found.<div>" : res.msg;
                self.dataMsg = $sce.trustAsHtml(self.dataMsg);
                angular.copy({}, self.filters);
                self.cols = res.columns;
                self.tableParams.settings({ dataset: res.data, total: res.total });
                self.tableParams.parameters.sorting = self.sorting;
            }
        });
    };

    self.menuClick = function (menu) {
        self.env_id = menu.id;
        self.datatableRefresh();
    };

    self.tabClick = function (tab) {
        self.repo_id = tab.id;
        self.sorting[tab.default_sort_col_name] = tab.default_sort_col_order;
        self.datatableRefresh();
    };

    self.dateChange = function (datevalue) {
        if (datevalue === null) datevalue = "";
        else datevalue = $filter('date')(datevalue, 'yyyy/MM/dd');
        self.date_val = "'" + datevalue + "'"; // db needs string in single quotes        
        self.datatableRefresh();
    };

} ]);