import React from "react";
import css from "./Loader.module.scss";

export default function Loader() {
  return (
    <div className={css.ringLoader}>
      Loading
      <span className={css.ringLoaderSpan}></span>
    </div>
  );
}
