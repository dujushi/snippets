# Front End Automation With Gulp

## Useful Resources
1. [Google Web Starter Kit](https://github.com/google/web-starter-kit/blob/master/gulpfile.babel.js)
2. [Gulp Docs](https://github.com/gulpjs/gulp/blob/master/docs/README.md)
3. [Gulp Recipes](https://github.com/gulpjs/gulp/tree/master/docs/recipes#recipes)
4. [Deploying to Azure](https://github.com/aranasoft/todo-azurewebsites/wiki/Deploying-to-Azure)

## How to
1. Install [Node.js](https://nodejs.org/en/)
2. Download this project, cd the directory 
2. Install all dependencies
```
npm install
```
3. Install bower components
```
./node_modules/.bin/bower install
```
4. Run Gulp
	- Development
	```
	./node_modules/.bin/gulp
	```
	- Production
	```
	./node_modules/.bin/gulp --production
	```

## Features
1. sass
	- parse sass
	- bundle
	- minify
	- source map for development
	- revision
2. react js
	- manage react lib with bower
	- parse react js with browserify and babel
	- bundle
	- uglify
	- source map for development
	- revision
3. html
	- dynamic js/css path
4. watch file changes, run related tasks and reload browser automatically