angular.module('app').factory('dataService', ['$http', '$q', function ($http, $q) {

    var dataService = {};
    dataService.getdata = function (service_url, env_id, repo_id, date_val) {
        var deferred = $q.defer();
        $http.get(service_url + "/getjson?type=data&env_id=" + env_id + "&repo_id=" + repo_id + "&date_val=" + date_val + "&request_id=" + new Date().getTime())
            .success(function (response) {
                var res = JSON.parse(response);
                deferred.resolve(res);
            })
            .error(function (error) {
                var error_msg = "Unexpected error occurred. Please verify the webserver and app settings again."
                if (error) error_msg = error;
                var res = { "total": 0, data: [], columns: [], msg: error_msg, "env_id": env_id, "repo_id": repo_id };
                deferred.resolve(res);
            });
        return deferred.promise;
    }
    return dataService;
} ]);