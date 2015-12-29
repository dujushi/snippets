"use strict";

var os = require('os');
var fs = require('fs');
var gulp = require('gulp');
var gulpLoadPlugins = require('gulp-load-plugins');
var plugins = gulpLoadPlugins();
var browserify = require('browserify');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var del = require('del');
var runSequence = require('run-sequence');
var objectAssign = require('object-assign');
var browser = os.platform() === 'linux' ? 'google-chrome' : (
  os.platform() === 'darwin' ? 'google chrome' : (
  os.platform() === 'win32' ? 'chrome' : 'firefox'));

var config = {
    production: !!plugins.util.env.production,
	port: 9005,
	devBaseUrl: 'http://localhost',
	paths: {
		mainJs: './src/main.js',
		appJs: './src/app/**/*.js',
		scss: './src/**/*.scss',
		html: './src/*.html',
		build: './build'
	}
}

gulp.task('connect', function() {
    return plugins.connect.server({
        root: ['build'],
        port: config.port,
        base: config.devBaseUrl,
        livereload: true
    });
});

gulp.task('open', function() {
    gulp.src('')
        .pipe(plugins.open({
            uri: config.devBaseUrl + ':' + config.port + '/',
            app: browser
        }));
});

gulp.task('html', function() {
    var handlebarOptions = {
        helpers: {
            versionPath: function(path, context) {
                return context.data.root[path];
            }
        }
    };
    var manifestCss = JSON.parse(fs.readFileSync(config.paths.build + '/rev-manifest-css.json', 'utf8'));
    var manifestJs = JSON.parse(fs.readFileSync(config.paths.build + '/rev-manifest-js.json', 'utf8'));
    return gulp.src(config.paths.html)
        .pipe(plugins.compileHandlebars(objectAssign(manifestCss, manifestJs), handlebarOptions))
        .pipe(gulp.dest(config.paths.build))
        .pipe(plugins.connect.reload());
});

gulp.task('js', function() {
    return browserify(config.paths.mainJs, {paths: ['./bower_components/react']})
        .transform("babelify", {presets: ["react"]})
        .bundle()
        .on('error', console.error.bind(console))
        .pipe(source('bundle.min.js'))
        .pipe(buffer())
        .pipe(plugins.if(!config.production, plugins.sourcemaps.init()))
        .pipe(plugins.uglify())
        .pipe(plugins.rev())
        .pipe(plugins.if(!config.production, plugins.sourcemaps.write('.')))
        .pipe(gulp.dest(config.paths.build))
        .pipe(plugins.rev.manifest(config.paths.build + "/rev-manifest-js.json", {cwd: config.paths.build + "/", base: config.paths.build + "/", merge: true}))
        .pipe(gulp.dest(config.paths.build));
});

gulp.task('scss', function () {
    return gulp.src(config.paths.scss)
        .pipe(plugins.if(!config.production, plugins.sourcemaps.init()))
        .pipe(plugins.sass().on('error', plugins.sass.logError))
        .pipe(plugins.concat('bundle.min.css'))
        .pipe(plugins.minifyCss())
        .pipe(plugins.rev())
        .pipe(plugins.if(!config.production, plugins.sourcemaps.write('.')))
        .pipe(gulp.dest(config.paths.build))
        .pipe(plugins.rev.manifest(config.paths.build + "/rev-manifest-css.json", {cwd: config.paths.build + "/", base: config.paths.build + "/", merge: true}))
        .pipe(gulp.dest(config.paths.build));
});

gulp.task('clean', function() {
    return del.sync(config.paths.build);
});

gulp.task('watch', function() {
	gulp.watch(config.paths.html, ['html']);
	gulp.watch(config.paths.scss, function(event) {
            runSequence('scss', 'html');
        });
	gulp.watch([config.paths.mainJs, config.paths.appJs], function(event) {
            runSequence('js', 'html');
        });
});

gulp.task('default', function() {
    if (config.production) {
        runSequence('clean', 'scss', 'js', 'html');
    } else {
        runSequence('clean', 'scss', 'js', 'html', 'connect', 'open', 'watch');
    }
});