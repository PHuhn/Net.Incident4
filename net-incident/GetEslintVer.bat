@REM GetEslintVer
@REM Note VER is reserved word
@SET VRSN=8.2.14
@SET VRSN=latest
REM npm install --save-dev @angular-eslint/builder@%VRSN% @angular-eslint/eslint-plugin@%VRSN% @angular-eslint/eslint-plugin-template@%VRSN% @angular-eslint/schematics@%VRSN% @angular-eslint/template-parser@%VRSN% --force
npm install --save-dev @angular-eslint/builder@%VRSN% @angular-eslint/eslint-plugin@%VRSN% @angular-eslint/eslint-plugin-template@%VRSN% @angular-eslint/schematics@%VRSN% @angular-eslint/template-parser@%VRSN% @typescript-eslint/eslint-plugin@%VRSN% @typescript-eslint/parser@%VRSN% eslint@%VRSN% --force
pause
