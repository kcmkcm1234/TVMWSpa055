
$(document).ready(function () {
    debugger;
    LoadSalesChart();
});

$(function () {
    debugger;
    SalesPurchaseChart();

});

function SalesPurchaseChart() {
    debugger;
    var mlist = SPResult.MonthlyItemList;
    var SPlabelList = [];
    var SalesList = [];
    var PurchaseList = [];
    for (i = 0; i < mlist.length; i++) {
        SPlabelList.push(mlist[i].Period);
        SalesList.push(roundoff(parseFloat(mlist[i].Sales)/1000));
        PurchaseList.push(roundoff(parseFloat(mlist[i].Purchase) / 1000));

    }
    createSalesPurchaseChart(SPlabelList, SalesList, PurchaseList);
}

function createSalesPurchaseChart(SPlabelList, SalesList, PurchaseList) {
    debugger;
    'use strict';

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    // -----------------------
    // - MONTHLY SALES CHART -
    // -----------------------

    // Get context with jQuery - using jQuery's .get() method.
  //  var salesPurchaseChartCanvas = $('#salesPurchaseChart').get(0).getContext('2d');
    // This will get the first returned node in the jQuery collection.
  //  var salesPurchaseChart = new Chart(salesPurchaseChartCanvas);
    if ($("#Invoice").prop('checked')) {
        var summaryType = $("#Invoice").val();
    }
    else {
        var summaryType = $("#Payment").val();
    }

    var salesPurchaseChartOptions = {
        // Boolean - If we should show the scale at all
        showScale: true,
        // Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: false,
        // String - Colour of the grid lines
        scaleGridLineColor: 'rgba(0,0,0,.05)',
        // Number - Width of the grid lines
        scaleGridLineWidth: 1,
        // Boolean - Whether to show horizontal lines (except X axis)
        scaleShowHorizontalLines: true,
        // Boolean - Whether to show vertical lines (except Y axis)
        scaleShowVerticalLines: true,
        // Boolean - Whether the line is curved between points
        bezierCurve: true,
        // Number - Tension of the bezier curve between points
        bezierCurveTension: 0.3,
        // Boolean - Whether to show a dot for each point
        pointDot: false,
        // Number - Radius of each point dot in pixels
        pointDotRadius: 4,
        // Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,
        // Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,
        // Boolean - Whether to show a stroke for datasets
        datasetStroke: true,
        // Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,
        // Boolean - Whether to fill the dataset with a color
        datasetFill: true,
        // String - A legend template
        legendTemplate: '<ul class=\'<%=name.toLowerCase()%>-legend\'><% for (var i=0; i<datasets.length; i++){%><li><span style=\'background-color:<%=datasets[i].lineColor%>\'></span><%=datasets[i].label%></li><%}%></ul>',
        // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
        maintainAspectRatio: true,
        // Boolean - whether to make the chart responsive to window resizing
        responsive: true
    };

    if (summaryType == "Invoice")
    {

        $('#lbltxt').text('Based On Invoice');
        $('#payment').hide();
        $('#invoice').show();


    var salesPurchaseChartData = {
        labels: SPlabelList,
        datasets: [
          {

              label: 'Sales',
              fillColor: 'rgba(206, 153, 71, 1)',//rgba(241, 155, 21, 0.62)
              strokeColor: 'rgb(210, 214, 222)',
              pointColor: 'rgb(81, 97, 225)',
              pointStrokeColor: '#c1c7d1',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgb(220,220,220)',
              data: SalesList,
              //backgroundColor: "rgba(81, 97, 225, 0.5)"
          },
          {

              label: 'Purchase',
              fillColor: 'rgba(48, 204, 132, 1)',//(10, 156, 43, 0.5)
              strokeColor: 'rgba(48, 204, 132, 1)',
              pointColor: '#3b8bba',
              pointStrokeColor: 'rgba(48, 204, 132, 1)',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgba(48, 204, 132, 1))',
              data: PurchaseList,
              //backgroundColor: "rgba(260,141,188,0.5)"
          }
        ]
    };
    var c = $('#salesPurchaseChart');
    var ct = c.get(0).getContext('2d');
    var ctx = document.getElementById("salesPurchaseChart").getContext("2d");
    /*********************/
    new Chart(ctx).Bar(salesPurchaseChartData, salesPurchaseChartOptions);
    }
    else if(summaryType == "Payment")
    {

        $('#lbltxt').text('Based On Payment');
        $('#payment').show();
        $('#invoice').hide();

        var salesPurchaseChartData = {
            labels: SPlabelList,
            datasets: [
              {

                  label: 'Sales',
                  fillColor: 'rgba(228, 102, 42, 0.89)',
                  strokeColor: 'rgb(210, 214, 222)',
                  pointColor: 'rgb(81, 97, 225)',
                  pointStrokeColor: '#c1c7d1',
                  pointHighlightFill: '#fff',
                  pointHighlightStroke: 'rgb(220,220,220)',
                  data: SalesList,
                  //backgroundColor: "rgba(81, 97, 225, 0.5)"
              },
              {

                  label: 'Purchase',
                  fillColor: 'rgba(82,166,216,1)',//rgba(91,202,151,1)
                  strokeColor: 'rgba(82,166,216,0.8)',
                  pointColor: '#3b8bba',
                  pointStrokeColor: 'rgba(82,166,216,0.8)',
                  pointHighlightFill: '#fff',
                  pointHighlightStroke: 'rgbargba(82,166,216,0.8)',
                  data: PurchaseList,
                  //backgroundColor: "rgba(260,141,188,0.5)"
              }
            ]
        };
        var c = $('#salesPurchaseChart');
        var ct = c.get(0).getContext('2d');
        var ctx = document.getElementById("salesPurchaseChart").getContext("2d");
        /*********************/
        new Chart(ctx).Bar(salesPurchaseChartData, salesPurchaseChartOptions);

    }
    //var options = {

        //animation: true,
        //tooltipTemplate: function (V) {
        //    debugger;
        //    return V.Sales + ":: ₹ " + V.Purchase * 1000
        //},
        //tooltipFillColor: "rgba(255,255,255,.89)",
        //tooltipFontColor: "rgba(1,1,1,1)",
        //tooltipCaretSize: 0,
        //tooltipFontSize: 14,
        //tooltipFontStyle: "thick",

  //  };
    //Get the context of the canvas element we want to select
    //var c = $('#salesPurchaseChart');
    //var ct = c.get(0).getContext('2d');
    //var ctx = document.getElementById("salesPurchaseChart").getContext("2d");
    ///*********************/
    //new Chart(ctx).Bar(salesPurchaseChartData, salesPurchaseChartOptions);


   
    //salesPurchaseChart.Line(salesPurchaseChartData, salesPurchaseChartOptions);



    // ---------------------------
    // - END MONTHLY SALES CHART -
    // ---------------------------


}
