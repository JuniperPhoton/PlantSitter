<?php

do
{
	$action=$_GET['action'];
	switch ($action) {
		case 'CheckUserExist':
			$email=$_POST['email'];
			$queryFind=$pdo->prepare('SELECT * FROM user WHERE email=:email');
			$queryFind->bindParam(':email',$email,PDO::PARAM_STR);
			$result=$queryFind->execute();
			if($result)
			{
				$user=$queryFind->fetch();
				if($user)
				{
					$ApiResult['isSuccessed']=true;
					$ApiResult['error_code']=0;
					$ApiResult['error_message']='';
					$ApiResult['isExist']=true;
					break;
				}
				else
				{
					$ApiResult['isSuccessed']=true;
					$ApiResult['error_code']=0;
					$ApiResult['error_message']='';
					$ApiResult['isExist']=false;
					break;
				}
			}
			else
			{
				$ApiResult['isSuccessed']=false;
				$ApiResult['error_code']=API_ERROR_DATABASE_ERROR;
				$ApiResult['error_message']='database error';
				break;
			}
			break;

		default:
			# code...
			break;
	}

}while(0);


?>