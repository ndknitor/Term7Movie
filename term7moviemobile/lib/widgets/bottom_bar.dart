import 'package:flutter/material.dart';
import 'package:term7moviemobile/utils/theme.dart';

class BottomBar extends StatefulWidget {

  const BottomBar({Key? key}) : super(key: key);

  @override
  State<BottomBar> createState() => _BottomBarState();
}

class _BottomBarState extends State<BottomBar> {
  int _currentIndex = 0;

  @override
  Widget build(BuildContext context) {
    return BottomNavigationBar(
      type: BottomNavigationBarType.fixed,
      backgroundColor: Colors.white,
      selectedItemColor: MyTheme.primaryColor,
      unselectedItemColor: MyTheme.bottomBarColor,
      currentIndex: _currentIndex,
      selectedFontSize: 14,
      // selectedLabelStyle: TextStyle(fontWeight: FontWeight.w600),
      unselectedFontSize: 0,
      onTap: (value) {
        // Respond to item press.
        setState(() => _currentIndex = value);
      },
      items: [
        BottomNavigationBarItem(
          label: 'Home',
          icon: Icon(Icons.home_rounded),
        ),
        BottomNavigationBarItem(
          label: 'Movies',
          icon: Icon(Icons.movie_filter),
        ),
        BottomNavigationBarItem(
          label: 'Places',
          icon: Icon(Icons.local_movies_rounded),
        ),
        BottomNavigationBarItem(
          label: 'Profile',
          icon: Icon(Icons.account_circle_rounded),
        ),
      ],
    );
  }
}
