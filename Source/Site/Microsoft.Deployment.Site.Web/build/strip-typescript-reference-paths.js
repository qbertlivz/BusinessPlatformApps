var gulp = require('gulp');
var strip = require('gulp-strip-comments');

gulp.task('strip-typescript-reference-paths', function (callback) {
    gulp.src('wwwroot/dist/**/*.ts').pipe(strip.text({ trim: true })).pipe(gulp.dest('wwwroot/dist'));
});