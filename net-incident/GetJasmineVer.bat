@REM GetJasmineVer
@REM Note VER is reserved word
@SET VRSN=3.10.2
@SET VRSN=latest
npm install --save-dev @types/jasmine@%VRSN% jasmine-core@%VRSN% karma@%VRSN% karma-chrome-launcher@%VRSN% karma-coverage@%VRSN% karma-jasmine@%VRSN% karma-jasmine-html-reporter@%VRSN% --force
pause

