import 'package:flutter/material.dart';
import 'package:term7moviemobile/libraries/HttpFetch.dart';
import 'package:term7moviemobile/resourses.dart';
import 'presenters/router.dart' as router;

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    config();
    return MaterialApp(
      onGenerateRoute: router.generateRoute,
      initialRoute: '/',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
    );
  }

  void config() {
    HttpFetch.host = Resourses.host;
  }
}
