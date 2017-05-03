var del = require('del');
var gulp = require('gulp');
var runSequence = require('run-sequence');

gulp.task('copy-apps', function () {
    return gulp.src('../../Apps/**/Web/**/*').pipe(gulp.dest('wwwroot/dist/Apps/'));
});

gulp.task('copy-sitecommon', function () {
    gulp.src('../../SiteCommon/Web/**/*').pipe(gulp.dest('wwwroot/dist/SiteCommon/Web'));
});

gulp.task('copy-src', function () {
    return gulp.src('src/**/*').pipe(gulp.dest('wwwroot/dist/'));
});

gulp.task('clean-dist', function () {
    return del(['wwwroot/dist']);
});

gulp.task('pre-build', function (callback) {
    runSequence('clean-dist', ['copy-apps', 'copy-sitecommon', 'copy-src'], callback);
});