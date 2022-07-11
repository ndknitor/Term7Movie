import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:term7moviemobile/utils/theme.dart';

class SocialButton extends StatelessWidget {
  final Function() onGoogleClick;

  const SocialButton({Key? key, required this.onGoogleClick}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: double.infinity,
      child: OutlinedButton.icon(
        onPressed: onGoogleClick,
        icon: SvgPicture.asset(
          "assets/images/google.svg",
          height: 24,
          width: 24,
        ),
        label: const Padding(
          padding: EdgeInsets.all(20),
          child: Text(
            "Sign In With Google",
            style: TextStyle(
              color: MyTheme.textColor,
              fontSize: 20,
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
      ),
    );
  }
}
