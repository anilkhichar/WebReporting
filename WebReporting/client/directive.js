angular.module('app').directive('layout', function () {
    return {
        scope: {
            menus: '=',
            application: '=',
            envid: '=',
            menuClick: '=',
            dateChange: '='
        },
        templateUrl: 'client/view/layout.ng.html',
        controller: ['$scope', '$element', '$attrs',
            function ($scope, $element, $attrs) {
                var self = $scope;
                self.format = "yyyy/MM/dd";
                self.altInputFormats = ['yyyy/M!/d!'];
                self.dateOptions = {
                    formatYear: 'yyyy',
                    maxDate: new Date(2050, 5, 22),
                    // minDate: new Date(),
                    startingDay: 1
                };
                self.datevalue = "";
                self.$watch('datevalue', function (date) {
                    if (date != "" && date !== undefined) // pass valid date value and null(if date is cleared)
                        self.dateChange(date);
                });
            }
        ]
    };
});

angular.module('app').directive('tabs', function () {
    return {
        scope: {
            tabs: '=',
            tabClick: '='            
        },
        templateUrl: 'client/view/tab.ng.html'        
    };
});