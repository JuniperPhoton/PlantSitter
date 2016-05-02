<?php 

//common error define
define('API_ERROR_DATABASE_ERROR', 100);
define('API_ERROR_ACTION_NOTEXIST', 101);
define('API_ERROR_ACCESS_TOKEN_INVAID', 102);

//user
define('USER_NOT_EXIST', 200);
define('USER_ALEADY_EXIST', 201);
define('EMAIL_OR_PWD_WRONG',202);

//Plan
define('PLAN_NOT_EXIST', 300);
define('LACK_SOME_PARAMS', 301);

//Timeline
define('LACK_PLANT_PARAM',400);
define('LACK_FILTER_VALUE',401);
define('LACK_FILTER_KIND',402);
define('TIME_FORMAT_WRONG',403);

//Plant
define('PLANT_NOT_EXIST',500);
define('PLANT_EXIST',501);
?>