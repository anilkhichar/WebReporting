angular.module('app', ['ngAnimate', 'angular.filter', 'ui.bootstrap', 'ngTable']);

angular.module('app').config(['$compileProvider', function ($compileProvider) {
  $compileProvider.debugInfoEnabled(false);
} ]);

angular.element(document).ready(function () {
    $.getJSON('/ReportingService.svc/getjson?type=setup&rnd=' + new Date().getTime(), function (json) {
        angular.module('app').value('setupjson', JSON.parse(json));

        // manual bootstrap
        angular.bootstrap($("div[id='app']"), ['app']);
    });
});