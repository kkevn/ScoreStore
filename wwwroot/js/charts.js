// load charts and packages
google.charts.load('current', { 'packages': ['corechart', 'timeline'] });

// draw win/loss donut chart
google.charts.setOnLoadCallback(drawDonutChart);

// draw win totals per game column chart
google.charts.setOnLoadCallback(drawColumnChartGames);

// draw win totals per user column chart
google.charts.setOnLoadCallback(drawColumnChartUsers);

// draw win streak area chart
google.charts.setOnLoadCallback(drawAreaChart);

// draw win history timeline
google.charts.setOnLoadCallback(drawTimeline);

// set chart height to 40% of total window height
var chartHeight = window.innerHeight * 0.4;

// render the win loss ratio donut chart
function drawDonutChart() {

    // initialize data table with user's total wins and losses from view bag
    var data = google.visualization.arrayToDataTable([
        ['Result', 'Count'],
        //['Wins', parseInt(@(ViewBag.Wins))],
        ['Wins', parseInt(ratioWins)],
        ['Losses', parseInt(ratioLosses)],
        //['Losses', parseInt(@(ViewBag.Losses))],
        ]);

    // set options for this chart
    var options = {
        title: 'Win/Loss Ratio',
        pieHole: 0.5,
        pieSliceTextStyle: {
            color: 'black',
        },
        legend: 'none',
        width: 300,
        //width: '100%',
        height: 300,
        //height: '100%',
        chartArea: {
            height: '100%',
            width: '100%',
            top: 48,
            left: 48,
            right: 16,
            bottom: 48
        },
    };

    // instantiate and draw the chart
    var chart = new google.visualization.PieChart(document.getElementById('donut_chart'));
    chart.draw(data, options);
}

// render the win totals column chart
function drawColumnChartGames() {

    // initialize empty data table with columns for user and their win counts for each game
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Game');
    data.addColumn('number', 'Wins');

    // retrieve win list from view data
    // has format: { GameName = ..., GameWins = ... }:{ GameName = ..., GameWins = ...}:{etc...}
    //var input = "@ViewData["columns"]";
    var input = columnInput;

    // add row to data table for each element in the split input
    input.split(':').forEach(function (s) {

        // parse current string by removing unnecessary characters, ex:
        //  given input: '{ GameName = Soccer, GameWins = 123 }'
        //  when parsed: 'Soccer,123'
        var parsed = s.replace("{ GameName = ", "");
        parsed = parsed.replace(" GameWins = ", "");
        parsed = parsed.replace(" }", "");

        // extract game title and win totals from parsed string
        var delimIndex = parsed.indexOf(',');
        var game = parsed.substring(0, delimIndex);
        var wins = parsed.substring(delimIndex + 1, parsed.length);

        data.addRow([game, parseInt(wins)]);   // add parsed values to data table
    });

    // set options for this chart
    var options = {
        title: 'Win Totals',
        chartArea: {
            height: '100%',
            width: '100%',
            top: 48,
            left: 48,
            right: 16,
            bottom: 48
        },
        //width: 800,
        width: '100%',
        //height: 400,
        //height: '100%',
        height: chartHeight,
        vAxis: {
            format: '#'
        }
    };

    // instantiate and draw the chart
    var chart = new google.visualization.ColumnChart(document.getElementById('column_chart_games'));
    chart.draw(data, options);
}

// render the win totals per user column chart
function drawColumnChartUsers() {

    // initialize empty data table with columns for users and their win counts
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'User');
    data.addColumn('number', 'Wins');

    // retrieve win list from view data
    // has format: { Profile = ..., Wins = ... }:{ Profile = ..., Wins = ...}:{etc...}
    var input = columnInput;

    // add row to data table for each element in the split input
    input.split(':').forEach(function (s) {

        // parse current string by removing unnecessary characters, ex:
        //  given input: '{ Profile = test_user, Wins = 123 }'
        //  when parsed: 'test_user,123'
        var parsed = s.replace("{ Profile = ", "");
        parsed = parsed.replace(" Wins = ", "");
        parsed = parsed.replace(" }", "");

        // extract user and win totals from parsed string
        var delimIndex = parsed.indexOf(',');
        var user = parsed.substring(0, delimIndex);
        var wins = parsed.substring(delimIndex + 1, parsed.length);

        data.addRow([user, parseInt(wins)]);   // add parsed values to data table
    });

    // set options for this chart
    var options = {
        title: 'Win Totals',
        chartArea: {
            height: '100%',
            width: '100%',
            top: 48,
            left: 48,
            right: 16,
            bottom: 48
        },
        //width: 800,
        width: '100%',
        //height: 400,
        //height: '100%',
        height: chartHeight,
        vAxis: {
            format: '#'
        }
    };

    // instantiate and draw the chart
    var chart = new google.visualization.ColumnChart(document.getElementById('column_chart_users'));
    chart.draw(data, options);
}

// render the win streak area chart
function drawAreaChart() {

    // initialize empty data table
    var data = new google.visualization.DataTable();

    // retrieve streak list from model and remove trailing comma
    //var input = ("@Model.StreakList").slice(0, -1);
    var input = (modelStreakList).slice(0, -1);

    // split input into array of users in the list
    var streakList = input.split(',');

    // create array containing set of each user found in streak list
    let uniqueUsers = Array.from([...new Set(streakList)]);

    // add column to data table for match index and each unique user
    data.addColumn('number', 'Index');
    for (const user of uniqueUsers) {
        data.addColumn('number', user);
    }

    // create and zero-fill array with length of unique users plus one for match index
    // array format: [index, user1, user2, ..., userN]
    var rows = Array(uniqueUsers.length + 1).fill(0);

    data.addRow(rows);  // add default/empty score row to data table

    // iterate over each user that won in the streak list
    for (let i = 0; i < streakList.length; i++) {

        // iterate over each unique user found in streak list
        for (let j = 0; j < uniqueUsers.length; j++) {

            // check if index matches current streak list user
            if (streakList[i] == uniqueUsers[j]) {
                rows[j + 1]++;      // increment score when users match
                rows[0] = i + 1;    // increment index
            }
        }
        data.addRow(rows);  // add current score state to data table
    }

    // set options for this chart
    var options = {
        title: 'Win Streaks',
        chartArea: {
            height: '100%',
            width: '100%',
            top: 48,
            left: 48,
            right: 16,
            bottom: 48
        },
        width: '100%',
        //width: 800,
        //height: '100%',
        //height: 400,
        height: chartHeight,
        hAxis: {
            title: 'Match History',
            titleTextStyle: {
                color: '#333'
            },
            textPosition: 'none'
        },
        vAxis: {
            format: '#',
            minValue: 0
        }
    };

    // instantiate and draw the chart
    var chart = new google.visualization.AreaChart(document.getElementById('area_chart'));
    chart.draw(data, options);
}

// render the win history timeline
function drawTimeline() {

    // initialize empty data table
    var data = new google.visualization.DataTable();

    // add columns to data table
    data.addColumn({ type: 'string', id: 'Result' });
    data.addColumn({ type: 'string', id: 'dummy bar tooltip' });
    data.addColumn({ type: 'string', role: 'tooltip' });
    data.addColumn({ type: 'date', id: 'Start' });
    data.addColumn({ type: 'date', id: 'End' });

    // retrieve streak list from model and remove trailing comma
    //var input = ("@Model.StreakList").slice(0, -1);
    var input = (modelStreakList).slice(0, -1);

    // split input into array of users in the list
    var streakList = input.split(',');

    // iterate over each result in the streak list
    for (let i = 0; i < streakList.length; i++) {

        // if current user in streak list iteration is this user, set result to a win
        //let result = streakList[i] == "@Model.Name" ? 'Win' : 'Loss';
        let result = streakList[i] == modelName ? 'Win' : 'Loss';

        // add a row to the data table with current status
        data.addRow([result, null, result, new Date(i, 0, 0), new Date(i + 1, 0, 0)]);
    }

    // set options for this chart
    var options = {
        //width: 705,
        height: 200,
        chartArea: {
            height: '100%',
            width: '100%',
            top: 48,
            left: 48,
            right: 16,
            bottom: 48
        },
        timeline: {
            groupByRowLabel: true,
            showRowLabels: true,
            showBarLabels: false
        },
        avoidOverlappingGridLines: false,
        hAxis: {
            format: " "
        }
    };

    // instantiate and draw the chart
    var chart = new google.visualization.Timeline(document.getElementById('timeline'));
    chart.draw(data, options);
}

// on window resize, redraw charts to dynamically fit on screen
window.onresize = function () {
    drawAreaChart();
    drawColumnChartGames();
    drawColumnChartUsers();
}