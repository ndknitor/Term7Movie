import 'package:flutter/material.dart';
import 'package:term7moviemobile/presenters/pages/Home.dart';

Route<dynamic> generateRoute(RouteSettings settings) {
  switch (settings.name) {
    case '/':
      return MaterialPageRoute(builder: (context) => Home());
    default:
      return MaterialPageRoute(builder: (context) => Home());
  }
}
