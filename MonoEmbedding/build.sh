export PKG_CONFIG_PATH=/Library/Frameworks/Mono.framework/Versions/Current/lib/pkgconfig/
cc main.c `pkg-config --cflags --libs mono-2`