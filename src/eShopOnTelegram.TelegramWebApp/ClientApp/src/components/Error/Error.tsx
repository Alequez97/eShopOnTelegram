import classNames from "classnames";
import React from "react";
import css from "./Error.module.scss";

export default function Error() {
  return (
    <>
      <div className={css.browser}>
        <div className={css.controls}>
          <i className={classNames(css.browserButton, css.browserClose)}></i>
          <i className={classNames(css.browserButton, css.browserMinimize)}></i>
          <i className={classNames(css.browserButton, css.browserMaximize)}></i>
        </div>

        <div className={css.eye}></div>
        <div className={css.eye}></div>
        <div className={css.mouth}>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
        </div>
      </div>
      <div className={css.errorMessage}>
        <p>
          Unfortunately, we're unable to fulfill your request. Try your request again
          and if the error continues, please contact <a href="#">support team</a>.
        </p>
      </div>
    </>
  );
}
