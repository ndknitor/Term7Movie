import 'package:flutter/material.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key}) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: ElevatedButton(
          child: Center(
            child: Text('Logout'),
          ),
          onPressed: () {
            AuthController.instance.signOut();
          },
        ),
      ),
    );
  }
}
