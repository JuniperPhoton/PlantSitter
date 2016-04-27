<?php 

do {
    $action = $_GET['action'];
    switch ($action) {
        case 'SetMainPlan':
            {
                $uid = $_GET['uid'];
                $gid = $_POST['gid'];

                $queryUpdate = $pdo->prepare('UPDATE user SET main_plan_id=:gid WHERE uid=:uid');
                $queryUpdate->bindParam(':uid', $uid, PDO::PARAM_INT);
                $queryUpdate->bindParam(':gid', $gid, PDO::PARAM_INT);
                $result = $queryUpdate->execute();
                if ($result) {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
            }
            break;
        case 'GetMainPlan':
            {
                $uid = $_GET['uid'];

                $queryUpdate = $pdo->prepare('SELECT main_plan_id FROM user WHERE uid=:uid');
                $queryUpdate->bindParam(':uid', $uid, PDO::PARAM_INT);
                $result = $queryUpdate->execute();
                if ($result) {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Gid'] = $result - fetch();
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
            }
            break;
        case 'GetAllPlans':
            {
                $uid = $_GET['uid'];
                $queryFind = $pdo->prepare('SELECT * FROM user_plan WHERE uid=:uid');
                $queryFind->bindParam(':uid', $uid, PDO::PARAM_INT);
                $result = $queryFind->execute();
                if ($result) {
                    $plans = $queryFind->fetchAll();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plans'] = $plans;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'GetPlan':
            {
                $gid = $_GET['gid'];
                $queryFind = $pdo->prepare('SELECT * FROM user_plan WHERE gid=:gid');
                $queryFind->bindParam(':gid', $gid, PDO::PARAM_INT);
                $result = $queryFind->execute();
                if ($result) {
                    $plan = $queryFind->fetch();
                    if ($plan) {
                        $ApiResult['isSuccessed'] = true;
                        $ApiResult['error_code'] = 0;
                        $ApiResult['error_message'] = '';
                        $ApiResult['Plan'] = $plan;
                        break;
                    } else {
                        $ApiResult['isSuccessed'] = false;
                        $ApiResult['error_code'] = PLAN_NOT_EXIST;
                        $ApiResult['error_message'] = 'Can not find the plan with this gid';
                        break;
                    }
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'DeletePlan':
            {
                $gid = $_GET['gid'];
                $queryFind = $pdo->prepare('DELETE FROM user_plan WHERE gid=:gid');
                $queryFind->bindParam(':gid', $gid, PDO::PARAM_INT);

                $result = $queryFind->execute();
                if ($result) {
                    $plan = $queryFind->fetch();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'AddPlan':
            {
                $pid = $_POST['pid'];
                $uid = $_GET['uid'];
                $name = $_POST['name'];
                $time = $_POST['time'];

                if ($pid == '' || $uid == '' || $name == '' || $time == '') {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = LACK_SOME_PARAMS;
                    $ApiResult['error_message'] = 'Lack some params.';
                    break;
                }

                $queryAdd = $pdo->prepare('INSERT INTO user_plan(uid,pid,name,time) VALUES(:uid,:pid,:name,:time)');
                $queryAdd->bindParam(':uid', $uid, PDO::PARAM_INT);
                $queryAdd->bindParam(':pid', $pid, PDO::PARAM_INT);
                $queryAdd->bindParam(':name', $name, PDO::PARAM_STR);
                $queryAdd->bindParam(':time', $time, PDO::PARAM_STR);

                $result = $queryAdd->execute();
                if ($result) {
                    $newid = $pdo->lastInsertId();
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plan'] = array('gid' => $newid, 'name' => $name, 'pid' => $pid, 'uid' => $uid, 'time' => $time);
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
            }
            break;
        default:
            #code...
            break;
    }

} while (0);


?>