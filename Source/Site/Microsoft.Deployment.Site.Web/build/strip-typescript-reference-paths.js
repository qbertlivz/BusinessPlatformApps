var gulp = require('gulp');
var runSequence = require('run-sequence');
var strip = require('gulp-strip-comments');

gulp.task('strip-typescript-reference-paths', function (callback) {
    gulp.src('wwwroot/dist/SiteCommon/Web/**/*.ts').pipe(strip.text()).pipe(gulp.dest('wwwroot/dist/SiteCommon/Web'));
});