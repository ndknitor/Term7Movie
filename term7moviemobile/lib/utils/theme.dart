import 'package:flutter/material.dart';

class MyTheme {
  static const primaryColor = Color(0xFF6346FA);
  static const backgroundColor = Color(0xFFFBFBFB);
  static const borderColor = Color(0xFFEFEFEF);
  static const textColor = Color(0xFF2E2E30);
  static const grayColor = Color(0xFF84878B);
  static const bottomBarColor = Color(0xFF84878B);
  static const infoColor = Color(0xFF0068FF);
  static const successColor = Color(0xFF54D62C);
  static const errorColor = Color(0xFFFF4842);
  static const warningColor = Color(0xFFFFC107);

  static final myLightTheme = ThemeData(
    primaryColor: primaryColor,
    scaffoldBackgroundColor: backgroundColor,
    textTheme: const TextTheme(
      subtitle1: TextStyle(color: textColor, inherit: true),
    ),
    brightness: Brightness.light,
    backgroundColor: Colors.white,
    fontFamily: 'Poppins',
    outlinedButtonTheme: OutlinedButtonThemeData(
      style: OutlinedButton.styleFrom(
        primary: primaryColor,
        side: const BorderSide(width: 2, color: borderColor),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12),
        ),
      ),
    ),
    appBarTheme: const AppBarTheme(
      color: primaryColor,
    ),
  );
}
