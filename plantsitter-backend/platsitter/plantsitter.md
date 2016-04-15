#API design

##User

###CheckUserExist (GET)

PARAM

- email

RETURN

- isExist

###GetSalt (GET)

PARAM

- email

RETURN 

- Salt

###UpdateUserName (POST)

PARAM

- email
- username

###Register (POST)

PARAM

- email
- password(which is after md5 from the raw one)

###Login (POST)

PARAM

- email
- password(md5(md5(pwdInRaw)+salt))

##Plant

###GetPlanInfo (GET)

PARAM
- pid

RETURN 

- Plant

###AddPlant (POST)

PARAM

- name_c
- name_e
- soil_moisture
- envi_moisture
- envi_temp
- light

RETURN 

- PlantID

###SearchPlant (GET)

PARAM

- pid

or

- name_e

or 

- name_c

RETURN

- Plant

##Plan

###GetAllPlns (GET)

PARAM

- uid

RETURN

- Plans

###GetPlan (GET)

PARAM

- gid

RETURN 

- Plan

###DeletePlan (GET)

PARAM

- gid

RETURN 

- Plan

###AddPlan (POST)

PARAM

- gid
- uid
- name

PARAM

- Plan