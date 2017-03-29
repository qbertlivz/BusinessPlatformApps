require('babel-polyfill');

var babel = require('gulp-babel');
var gulp = require('gulp');
var sourcemaps = require('gulp-sourcemaps');
var typescript = require('gulp-typescript');

gulp.task('build-typescript', function () {
    return gulp.src(['wwwroot/**/*.ts', 'typings/**.*'])
        //.pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(typescript({
            emitDecoratorMetadata: true,
            experimentalDecorators: true,
            noImplicitAny: true,
            removeComments: true,
            //sourceMap: true,
            target: 'es6'
        }))
        .pipe(babel({
            plugins: ['transform-runtime'],
            presets: ['es2015']
        }))
        //.pipe(sourcemaps.write('.', {
        //    includeContent: false,
        //    sourceRoot: '/'
        //}))
        .pipe(gulp.dest('wwwroot/'));
});
