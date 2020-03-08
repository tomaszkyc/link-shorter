$(function () {
    console.log('Greetings from DashboardStatistics.js start');

    showHideElements();




    renderActualMonthActivity();
    renderBrowserStatisticsChart();
    renderTopPlatformsChart();






    console.log('Greetings from DashboardStatistics.js finished');
});



/**
 *  @description function show or hide elements after page is loaded
 * */
function showHideElements() {

    let elementsToHide = [
        'top-browser-wrapper',
        'top-platforms-chart-wrapper',
        'activity-from-actual-month-chart-wrapper'
    ];

    let elementsToShow = [];

    elementsToHide.forEach(function (element) {
        $('#' + element).hide();
    });


    elementsToShow.forEach(function (element) {
        $('#' + element).show();
    });
}


/**
 *@description function generate dashboard for actual month activity
 */
function renderActualMonthActivity() {

    var jqxhr = $.ajax('api/v1/statistics/actual-month')
        .done(function (data) {
            console.log(data);
            handleCreatingActualMonthStatisticsChart(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failure on loading browser statistics');
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            $('#activity-from-actual-month-chart-wrapper').hide();
        })
        .always(function () {

        });


}



function handleCreatingActualMonthStatisticsChart(dataFromAPI) {

    console.log('handleCreatingActualMonthStatisticsChart: ');
    console.log(dataFromAPI);


    if ($.isEmptyObject(dataFromAPI) || dataFromAPI.length == 0) {
        $('#activity-from-actual-month-chart-wrapper').hide();
    }
    else {
        $('#activity-from-actual-month-chart-wrapper').show();
    }

    let chartLabels = [];
    let chartData = [];

    dataFromAPI.forEach(function (element) {

        chartLabels.push(element['_data']);
        chartData.push(element['_category']);

    });


    var options = {
        noData: {
            text: 'Sorry but there was no activity for any of your links.',
            align: 'center',
            verticalAlign: 'middle',
            offsetX: 0,
            offsetY: 0,
            style: {
                color: undefined,
                fontSize: '14px',
                fontFamily: undefined
            }
        },
        chart: {
            height: 350,
            type: "area",
            zoom: {
                enabled: false
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: "straight"
        },
        series: [
            {
                name: "Number of entrance for all your links",
                data: chartLabels
            }
        ],
        labels: chartData,
        xaxis: {
            type: "date"
        },
        yaxis: {
            opposite: true
        },
    };

    var chart = new ApexCharts(document.querySelector("#activity-from-actual-month-chart"), options);


    chart.render().then(() => {
        const lowest = chart.getLowestValueInSeries(0)
        const highest = chart.getHighestValueInSeries(0)

        console.log(lowest)
        chart.addPointAnnotation({
            x: new Date(chart.w.globals.seriesX[0][chart.w.globals.series[0].indexOf(lowest)]).getTime(),
            y: lowest,
            label: {
                text: 'Lowest: ' + lowest,
                offsetY: 2
            },
        })

        chart.addPointAnnotation({
            x: new Date(chart.w.globals.seriesX[0][chart.w.globals.series[0].indexOf(highest)]).getTime(),
            y: highest,
            label: {
                text: 'Highest: ' + highest,
                offsetY: 2
            },
        })
    });



}



function renderBrowserStatisticsChart() {

    var jqxhr = $.ajax('api/v1/statistics/by-browser')
        .done(function (data) {
            console.log(data);
            handleCreatingBrowserStatisticsChart(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failure on loading browser statistics');
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            $('#top-browser-wrapper').hide();
        })
        .always(function () {

        });

}


function handleCreatingBrowserStatisticsChart(dataFromApi) {

    let seriesData = [];
    let labelsData = [];

    if ($.isEmptyObject(dataFromApi) || dataFromApi.length == 0) {
        $('#top-browser-wrapper').hide();
    }
    else {
        $('#top-browser-wrapper').show();
    }


    dataFromApi.forEach(function (element) {

        seriesData.push(element.count);
        labelsData.push(element.label);

    });

    var options = {
        chart: {
            height: 350,
            type: 'pie',
            events: {
                click: function (event, chartContext, config) {
                    console.log("click");
                },
                dataPointSelection: function (event, chartContext, config) {
                    console.log("dataPointSelection");
                }
            }
        },
        series: seriesData,
        labels: labelsData,
        colors: ['#007bff', '#00e396'],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200,
                    events: {
                        click: function (event, chartContext, config) {
                            console.log("test click");
                        },
                        dataPointSelection: function (event, chartContext, config) {
                            console.log("test dps");
                        }
                    }
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'center'
                }
            }
        }]

    }

    var chart = new ApexCharts(
        document.querySelector("#top-browsers-chart"),
        options
    );

    chart.render();
}



function renderTopPlatformsChart() {


    var jqxhr = $.ajax('api/v1/statistics/by-platform')
        .done(function (data) {
            console.log(data);
            handleCreatingTopPlatformsChart(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failure on loading platform statistics');
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
            $('#top-platforms-chart-wrapper').hide();
        })
        .always(function () {

        });

}

function handleCreatingTopPlatformsChart(dataFromApi) {

    let seriesData = [];
    let labelsData = [];

    if ($.isEmptyObject(dataFromApi) || dataFromApi.length == 0) {
        $('#top-platforms-chart-wrapper').hide();
    }
    else {
        $('#top-platforms-chart-wrapper').show();
    }


    dataFromApi.forEach(function (element) {

        seriesData.push(element.count);
        labelsData.push(element.label);

    });

    var options = {
        chart: {
            height: 350,
            type: 'pie',
            events: {
                click: function (event, chartContext, config) {
                    console.log("click");
                },
                dataPointSelection: function (event, chartContext, config) {
                    console.log("dataPointSelection");
                }
            }
        },
        series: seriesData,
        labels: labelsData,
        //colors: ['#007bff', '#00e396'],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200,
                    events: {
                        click: function (event, chartContext, config) {
                            console.log("test click");
                        },
                        dataPointSelection: function (event, chartContext, config) {
                            console.log("test dps");
                        }
                    }
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'center'
                }
            }
        }]

    }

    var chart = new ApexCharts(
        document.querySelector("#top-platforms-chart"),
        options
    );

    chart.render();
}