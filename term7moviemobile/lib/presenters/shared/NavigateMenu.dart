import 'package:flutter/material.dart';

class Menu extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => MenuState();
}

class MenuState extends State<Menu> {
  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        padding: EdgeInsets.zero,
        children: <Widget>[
          DrawerHeader(
            child: Text("Dit me may"),
            decoration: BoxDecoration(
              color: Colors.blue,
            ),
          ),
          ListTile(
            title: Text("Shop"),
            onTap: () {
              Navigator.popAndPushNamed(context, '/shop');
            },
          ),
          ListTile(
            title: Text("About"),
            onTap: () {
              Navigator.popAndPushNamed(context, "/about");
            },
          ),
          ListTile(
            title: Text("Contact"),
          ),
          ListTile(
            title: Text("Cart"),
          ),
        ],
      ),
    );
  }
}
