import 'package:flutter/material.dart';
import 'package:date_picker_timeline/date_picker_timeline.dart';
import 'package:date_picker_timeline/extra/style.dart';
import 'package:term7moviemobile/utils/theme.dart';

class MyDatePicker extends StatelessWidget {
  const MyDatePicker({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          DatePicker(
            DateTime.now(),
            initialSelectedDate: DateTime.now(),
            height: 94,
            selectedTextColor: MyTheme.primaryColor,
            deactivatedColor: MyTheme.textColor,
            selectionColor: Colors.black.withOpacity(0.1),
            dateTextStyle: defaultDateTextStyle.copyWith(
              color: MyTheme.textColor,
            ),
            dayTextStyle:
            defaultDayTextStyle.copyWith(color: MyTheme.textColor),
            monthTextStyle:
            defaultMonthTextStyle.copyWith(color: MyTheme.textColor),
          )
        ],
      ),
    );
  }
}
