class Constants {
  static const String baseApiUrl = "https://api.fcinema.tk/api/v1/";
  static const String defaultAvatar = "assets/images/default-avatar.jpg";
  static const String svgSeatVip = '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">'
      + '<path d="M20.9294 11.3794V19.6179H0.525391V11.3794C0.525585 10.3174 1.35451 9.43446 2.42845 9.35242C2.48102 9.34859 2.53417 9.34668 2.58791 9.34668C3.72664 9.34649 4.64985 10.2566 4.64985 11.3794V15.9805H16.8052V11.3794C16.8052 10.2566 17.7282 9.34668 18.8671 9.34668C18.9439 9.34668 19.0209 9.3507 19.0974 9.35892C20.1407 9.47462 20.9294 10.3444 20.9294 11.3794Z" fill="#FBFBFB"/>'
      + '<path d="M20.135 19.6191V19.6413L19.6383 23.7071H17.3033L16.8066 19.6413V19.6191H20.135Z" fill="#FBFBFB"/>'
      + '<path d="M19.1023 4.00421V9.32109L19.0976 9.35934C19.0212 9.35112 18.9442 9.3471 18.8674 9.3471C17.7284 9.3471 16.8054 10.257 16.8054 11.3798V15.9809H4.65009V11.3798C4.65009 10.257 3.72688 9.34691 2.58815 9.3471C2.53442 9.3471 2.48126 9.34902 2.42869 9.35284L2.42578 9.32109V4.00421C2.42578 2.92274 3.31504 2.0459 4.41226 2.0459H6.75471V3.88373C6.75471 4.15414 6.97702 4.3733 7.25132 4.3733H14.204C14.4783 4.3733 14.7006 4.15414 14.7006 3.88373V2.0459H17.1158C18.2128 2.0459 19.1023 2.92274 19.1023 4.00421ZM11.845 9.15223L12.9624 8.0786L11.4185 7.85714L10.7277 6.47753L10.0369 7.85714L8.49287 8.0786L9.61026 9.15223L9.34663 10.6684L10.7277 9.95257L12.1089 10.6684L11.845 9.15223Z" fill="#FBFBFB"/>'
      + '<path d="M14.7037 2.04707V3.8849C14.7037 4.15532 14.4814 4.37448 14.2071 4.37448H7.25443C6.98013 4.37448 6.75781 4.15532 6.75781 3.8849V0.784499C6.75781 0.514084 6.98013 0.294922 7.25443 0.294922H14.2071C14.4814 0.294922 14.7037 0.514084 14.7037 0.784499V2.04707Z" fill="#FBFBFB"/>'
      + '<path d="M12.6362 8.18247L11.7405 9.0431L11.6829 9.09838L11.6966 9.17698L11.9085 10.395L10.796 9.81843L10.727 9.78266L10.658 9.81844L9.54557 10.395L9.75736 9.17696L9.77103 9.09837L9.7135 9.0431L8.81779 8.18247L10.0575 8.00466L10.1352 7.99352L10.1703 7.92334L10.727 6.81159L11.2837 7.92334L11.3188 7.99352L11.3965 8.00466L12.6362 8.18247Z" fill="#FBFBFB" stroke="#9A9FA5" stroke-width="0.3"/>'
      + '<path d="M4.57832 19.6191V19.6413L4.0817 23.7071H1.74662L1.25 19.6413V19.6191H4.57832Z" fill="#FBFBFB"/>'
      + '<path d="M14.2048 0H7.25218C6.81356 0.000573726 6.45817 0.350928 6.45759 0.783324V1.75215H4.41311C3.15197 1.75368 2.13002 2.76114 2.12866 4.00421V9.0983C1.02679 9.31861 0.233365 10.2714 0.228516 11.3798V19.6183C0.228516 19.7806 0.361982 19.912 0.526487 19.912H0.983337L1.45105 23.7414C1.46909 23.8891 1.59616 24 1.74689 24H4.08197C4.23289 24 4.35996 23.8891 4.3778 23.7414L4.84552 19.912H16.5395L17.0072 23.7414C17.0253 23.8891 17.1522 24 17.3031 24H19.638C19.7887 24 19.9158 23.8891 19.9338 23.7414L20.4015 19.912H20.9305C21.0952 19.912 21.2285 19.7806 21.2285 19.6183V11.3798C21.2303 10.7623 20.9814 10.1698 20.5373 9.73456C20.2244 9.42723 19.831 9.21209 19.4011 9.11321V4.00421C19.3996 2.76114 18.3776 1.75368 17.1167 1.75215H14.9994V0.783324C14.999 0.350928 14.6435 0.000573726 14.2048 0ZM7.05353 0.783324C7.05353 0.675081 7.14257 0.587493 7.25218 0.587493H14.2048C14.3146 0.587493 14.4035 0.675081 14.4035 0.783324V3.88373C14.4035 3.99197 14.3146 4.07956 14.2048 4.07956H7.25218C7.14257 4.07956 7.05353 3.99197 7.05353 3.88373V0.783324ZM3.81756 23.4125H2.01091L1.58335 19.912H4.24531L3.81756 23.4125ZM19.3736 23.4125H17.5669L17.1395 19.912H19.8013L19.3736 23.4125ZM19.0663 9.65137C19.4632 9.69593 19.8331 9.87187 20.116 10.1501C20.4479 10.4754 20.6339 10.9183 20.6326 11.3798V19.3245H0.824458V11.3798C0.827756 10.4727 1.53505 9.71926 2.45205 9.64582C2.49686 9.64238 2.54303 9.64085 2.58901 9.64085C3.56323 9.64085 4.35297 10.4194 4.35297 11.3798V15.9809C4.35297 16.1432 4.48644 16.2746 4.65094 16.2746H16.8063C16.9708 16.2746 17.1042 16.1432 17.1042 15.9809V11.3798C17.1052 10.4198 17.8944 9.64181 18.8682 9.64085C18.9344 9.64085 19.0005 9.64429 19.0663 9.65137ZM17.1167 2.33964C18.0486 2.34079 18.804 3.08529 18.8052 4.00421V9.05489C17.5275 9.08969 16.5099 10.1199 16.5083 11.3798V15.6871H4.94892V11.3798C4.94911 10.1467 3.97314 9.12794 2.72461 9.05737V4.00421C2.72558 3.08529 3.48098 2.34079 4.41311 2.33964H6.45759V3.88373C6.45817 4.31631 6.81356 4.66667 7.25218 4.66705H14.2048C14.6435 4.66667 14.999 4.31631 14.9994 3.88373V2.33964H17.1167Z" fill="#9A9FA5"/>'
      + '</svg>';
  static const String svgSeatNormal = '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">'
      + '<path d="M20.9311 11.3799V19.6183H0.5271V11.3799C0.527294 10.3179 1.35622 9.43495 2.43016 9.35291C2.48273 9.34908 2.53588 9.34717 2.58962 9.34717C3.72835 9.34698 4.65156 10.2571 4.65156 11.3799V15.9809H16.8069V11.3799C16.8069 10.2571 17.7299 9.34717 18.8688 9.34717C18.9456 9.34717 19.0227 9.35118 19.0991 9.35941C20.1424 9.47511 20.9311 10.3449 20.9311 11.3799Z" fill="#FBFBFB"/>'
      + '<path d="M20.135 19.6191V19.6413L19.6383 23.7071H17.3033L16.8066 19.6413V19.6191H20.135Z" fill="#FBFBFB"/>'
      + '<path d="M19.082 4.00421V9.32109L19.0774 9.35934C19.0009 9.35112 18.9239 9.3471 18.8471 9.3471C17.7082 9.3471 16.7852 10.257 16.7852 11.3798V15.9809H4.62983V11.3798C4.62983 10.257 3.70662 9.34691 2.56789 9.3471C2.51415 9.3471 2.461 9.34902 2.40843 9.35284L2.40552 9.32109V4.00421C2.40552 2.92274 3.29478 2.0459 4.39199 2.0459H6.73444V3.88373C6.73444 4.15414 6.95676 4.3733 7.23106 4.3733H14.1837C14.458 4.3733 14.6803 4.15414 14.6803 3.88373V2.0459H17.0955C18.1926 2.0459 19.082 2.92274 19.082 4.00421Z" fill="#FBFBFB"/>'
      + '<path d="M14.7022 2.04585V3.88368C14.7022 4.1541 14.4799 4.37326 14.2056 4.37326H7.25296C6.97866 4.37326 6.75635 4.1541 6.75635 3.88368V0.783279C6.75635 0.512864 6.97866 0.293701 7.25296 0.293701H14.2056C14.4799 0.293701 14.7022 0.512864 14.7022 0.783279V2.04585Z" fill="#FBFBFB"/>'
      + '<path d="M4.57905 19.6184V19.6406L4.08243 23.7064H1.74735L1.25073 19.6406V19.6184H4.57905Z" fill="#FBFBFB"/>'
      + '<path d="M14.2048 0H7.25218C6.81356 0.000573726 6.45817 0.350928 6.45759 0.783324V1.75215H4.41311C3.15197 1.75368 2.13002 2.76114 2.12866 4.00421V9.0983C1.02679 9.31861 0.233365 10.2714 0.228516 11.3798V19.6183C0.228516 19.7806 0.361982 19.912 0.526487 19.912H0.983337L1.45105 23.7414C1.46909 23.8891 1.59616 24 1.74689 24H4.08197C4.23289 24 4.35996 23.8891 4.3778 23.7414L4.84552 19.912H16.5395L17.0072 23.7414C17.0253 23.8891 17.1522 24 17.3031 24H19.638C19.7887 24 19.9158 23.8891 19.9338 23.7414L20.4015 19.912H20.9305C21.0952 19.912 21.2285 19.7806 21.2285 19.6183V11.3798C21.2303 10.7623 20.9814 10.1698 20.5373 9.73456C20.2244 9.42723 19.831 9.21209 19.4011 9.11321V4.00421C19.3996 2.76114 18.3776 1.75368 17.1167 1.75215H14.9994V0.783324C14.999 0.350928 14.6435 0.000573726 14.2048 0ZM7.05353 0.783324C7.05353 0.675081 7.14257 0.587493 7.25218 0.587493H14.2048C14.3146 0.587493 14.4035 0.675081 14.4035 0.783324V3.88373C14.4035 3.99197 14.3146 4.07956 14.2048 4.07956H7.25218C7.14257 4.07956 7.05353 3.99197 7.05353 3.88373V0.783324ZM3.81756 23.4125H2.01091L1.58335 19.912H4.24531L3.81756 23.4125ZM19.3736 23.4125H17.5669L17.1395 19.912H19.8013L19.3736 23.4125ZM19.0663 9.65137C19.4632 9.69593 19.8331 9.87187 20.116 10.1501C20.4479 10.4754 20.6339 10.9183 20.6326 11.3798V19.3245H0.824458V11.3798C0.827756 10.4727 1.53505 9.71926 2.45205 9.64582C2.49686 9.64238 2.54303 9.64085 2.58901 9.64085C3.56323 9.64085 4.35297 10.4194 4.35297 11.3798V15.9809C4.35297 16.1432 4.48644 16.2746 4.65094 16.2746H16.8063C16.9708 16.2746 17.1042 16.1432 17.1042 15.9809V11.3798C17.1052 10.4198 17.8944 9.64181 18.8682 9.64085C18.9344 9.64085 19.0005 9.64429 19.0663 9.65137ZM17.1167 2.33964C18.0486 2.34079 18.804 3.08529 18.8052 4.00421V9.05489C17.5275 9.08969 16.5099 10.1199 16.5083 11.3798V15.6871H4.94892V11.3798C4.94911 10.1467 3.97314 9.12794 2.72461 9.05737V4.00421C2.72558 3.08529 3.48098 2.34079 4.41311 2.33964H6.45759V3.88373C6.45817 4.31631 6.81356 4.66667 7.25218 4.66705H14.2048C14.6435 4.66667 14.999 4.31631 14.9994 3.88373V2.33964H17.1167Z" fill="#9A9FA5"/>'
      + '</svg>';
  static const String defaultImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSBCGH3ttKv97u7GnSxpSM4_ijDWF7yPb7GKmva3LU4_ZDJ-X2Jnfw2DVcwgsqmdome8Uo&usqp=CAU&fbclid=IwAR2ULi_nXOW9Lz9mbL-cmlIDwb7Rh8LnDtmtrb3u6pmM7JxN7IMBbktl5PE";
}
